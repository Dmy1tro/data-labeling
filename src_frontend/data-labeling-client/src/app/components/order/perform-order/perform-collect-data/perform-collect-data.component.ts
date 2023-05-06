import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { IOrder } from 'src/app/domain/responses/order';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { DataService } from '../../data.service';

@Component({
  selector: 'app-perform-collect-data',
  templateUrl: './perform-collect-data.component.html',
  styleUrls: ['./perform-collect-data.component.css']
})
export class PerformCollectDataComponent implements OnInit, OnDestroy {

  @Input() order: IOrder
  @Output() orderChanged = new EventEmitter()
  dataForm: FormGroup
  files: Blob[] = []
  loading = false

  private destroy$ = new Subject()

  constructor(private fb: FormBuilder,
              private dataService: DataService,
              private toastr: ToastrService,
              private router: Router,
              public formValidator: FormValidationService) { }

  ngOnInit(): void {
    this.createForm()
  }

  createForm() {
    this.dataForm = this.fb.group({
      imageFiles: [null, [Validators.required]]
    })
  }

  onSubmit() {
    console.log('valid', this.dataForm.valid);
    
    if (this.dataForm.invalid) {
      this.dataForm.markAllAsTouched()
      return
    }

    this.loading = true
    const formData = new FormData()

    formData.append('orderId', this.order.id.toString())
    this.files.forEach(f => formData.append('imageFiles', f))

    this.dataService.uploadRawData(formData)
      .pipe(takeUntil(this.destroy$), finalize(() => this.loading = false))
      .subscribe(
        res => {
          this.toastr.success('Success', 'Data has been uploaded successfuly. Verify your balance')

          if (this.order.datSetRequiredCount - this.order.currentProgress === this.files.length) {
            this.router.navigate(FrontUrl.ordersForPerformerPage())
          }

          this.orderChanged.next()
          this.resetForm()
        },
        err => {
          this.toastr.error(err.error.detail)
          console.log('error', err);
        }
      )
  }

  onFileChange(input) {
    if (input.target.files && input.target.files[0]) {
      for (let item of input.target.files) {
        this.files.push(item)
      }
    }
  }

  resetForm() {
    this.dataForm.reset()
    this.files = []
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
