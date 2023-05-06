import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { OrderPriority } from 'src/app/domain/enums/order-priority';
import { OrderType } from 'src/app/domain/enums/order-type';
import { convertToISOFormat } from 'src/app/shared/helpers/date-helper';
import { enumSelector } from 'src/app/shared/helpers/enum-selector';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { GetOrderPriceRequest, OrderService } from '../order.service';
import { COMMA, ENTER, SPACE } from '@angular/cdk/keycodes';
import { toTypeTitle } from 'src/app/shared/helpers/name-helper';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css'],
  providers: [DatePipe]
})
export class CreateOrderComponent implements OnInit, OnDestroy {

  @ViewChild('imageFiles') imageFiles: ElementRef

  orderForm: FormGroup
  files: Blob[] = []
  price: number = 0
  orderTypes = enumSelector(OrderType)
  orderPriorities = enumSelector(OrderPriority)
  priceLoading = false
  loading = false
  readonly separatorKeysCodes = [ENTER, COMMA, SPACE] as const;

  private labelOrder = false
  private destroy$ = new Subject<void>()

  constructor(private dialogRef: MatDialogRef<CreateOrderComponent>,
    private fb: FormBuilder,
    private orderService: OrderService,
    private toastr: ToastrService,
    private datePipe: DatePipe,
    public formValidator: FormValidationService) { }

  ngOnInit(): void {
    this.createForm()
  }

  createForm() {
    this.orderForm = this.fb.group({
      name: [null, [Validators.required, Validators.maxLength(100)]],
      requirements: [null, [Validators.required, Validators.maxLength(500)]],
      datSetRequiredCount: [null, [Validators.required]],
      type: [null, [Validators.required]],
      priority: [null, [Validators.required]],
      deadline: [null, [Validators.required, (control: AbstractControl) => {
        if (control.value && Date.parse(control.value) < Date.now()) {          
          return { 'invalidDate': true }
        }

        return null
      }]]
    })

    this.orderForm.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          if (this.orderForm.valid) {
            this.getPrice()
          } else {
            this.price = 0
          }
        },
        err => {
          console.log('error', err);
        }
      )
  }

  get isLabelOrder() {
    return this.labelOrder
  }

  onTypeChange($event) {
    const type = this.orderForm.get('type').value

    if (type && type === OrderType.LabelData) {
      this.labelOrder = true
      this.orderForm.addControl('variants', new FormControl([],
        [
          (control: AbstractControl) => {
            if (control.value && control.value.length && control.value.length > 0) {
              return null
            }

            return { 'required': true }
          }]))
      this.orderForm.addControl('imageFiles', new FormControl(null, [
        (contorl: AbstractControl) => {
          if (contorl.value && contorl.value.length && contorl.value.length > 0) {
            return null
          }

          return { 'required': true }
        }]))
    } else {
      this.labelOrder = false
      this.orderForm.removeControl('imageFiles')
      this.orderForm.removeControl('variants')
    }
  }

  onFileChange(input) {
    if (input.target.files && input.target.files[0]) {
      for (let item of input.target.files) {
        this.files.push(item)
      }
    }

    this.orderForm.patchValue({
      datSetRequiredCount: this.files.length
    })
  }

  getPrice() {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched()
      return
    }
    this.priceLoading = true
    const formValue = this.orderForm.value
    const request = new GetOrderPriceRequest()
    request.datSetRequiredCount = formValue.datSetRequiredCount
    request.deadline = convertToISOFormat(formValue.deadline, this.datePipe)
    request.priority = formValue.priority
    request.type = formValue.type

    this.orderService.getPrice(request)
      .pipe(takeUntil(this.destroy$), finalize(() => this.priceLoading = false))
      .subscribe(
        res => {
          this.price = res.payload
        },
        err => {
          this.toastr.error(err.error.detail, 'Error')
          console.log(err);
        }
      )
  }

  addVariant(event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if (value) {
      const variants = this.orderForm.get('variants').value
      variants.push(value)
      this.orderForm.patchValue({
        variants: variants
      })
      event.input.value = ''
    }
  }

  removeVariant(variant) {
    const variants = this.orderForm.get('variants').value.filter(v => v !== variant)
    this.orderForm.patchValue({
      variants: variants
    })
  }

  onSubmit() {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched()
      return
    }

    this.loading = true
    const formValue = this.orderForm.value
    const formData = new FormData()

    formData.append('name', formValue.name)
    formData.append('requirements', formValue.requirements)
    if (formValue.variants) {
      formValue.variants.forEach(v => formData.append('variants', v))
    }
    formData.append('datSetRequiredCount', formValue.datSetRequiredCount)
    formData.append('type', formValue.type)
    formData.append('priority', formValue.priority)
    formData.append('deadline', convertToISOFormat(formValue.deadline, this.datePipe))
    this.files.forEach(f => formData.append('imageFiles', f))

    this.orderService.create(formData)
      .pipe(takeUntil(this.destroy$), finalize(() => this.loading = false))
      .subscribe(
        res => {
          this.dialogRef.close(true)
        },
        err => {
          this.toastr.error(err.error.detail)
          console.log(err);
        }
      )
  }

  toTypeTitle = toTypeTitle

  resetForm() {
    this.createForm()
    this.price = 0
    this.labelOrder = false
    if (this.imageFiles) this.imageFiles.nativeElement.value = null
    this.files = []
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
