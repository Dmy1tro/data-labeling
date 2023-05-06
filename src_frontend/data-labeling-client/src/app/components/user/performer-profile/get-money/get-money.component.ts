import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IPerformer } from 'src/app/domain/responses/performer';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { PaymentService } from '../../payment.service';
import { UserService } from '../../user.service';

@Component({
  selector: 'app-get-money',
  templateUrl: './get-money.component.html',
  styleUrls: ['./get-money.component.css']
})
export class GetMoneyComponent implements OnInit, OnDestroy {

  paymentForm: FormGroup
  performer: IPerformer

  private destroy$ = new Subject()

  constructor(private dialogRef: MatDialogRef<GetMoneyComponent>,
              private fb: FormBuilder,
              private userService: UserService,
              private paymentService: PaymentService,
              private toastr: ToastrService,
              public formValidator: FormValidationService) { }

  ngOnInit(): void {
    this.getPerformer()
  }

  getPerformer() {
    this.userService.getPerformerInfo()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.performer = res.payload
          this.createForm()
        },
        err => console.log(err)
      )
  }

  createForm() {
    this.paymentForm = this.fb.group({
      bankCardNumber: [null, [Validators.required, (control: AbstractControl) => {
        if (control.value && control.value.toString().length !== 16) {
          return { 'invalidBankCardNumber': true }
        }

        return null
      }]],
      money: [null, [Validators.required, (control: AbstractControl) => {
        if (control.value && +control.value > this.performer.balance) {
          return { 'invalidWithdrawableAmount': true }
        }

        if (control.value && +control.value <= 0) {
          return { 'zeroWithdrawableAmount': true }
        }

        if (control.value != null && Number.parseFloat(control.value) === 0) {
          return { 'zeroWithdrawableAmount': true }
        }
        
        return null
      }]]
    })
  }

  onSubmit() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched()
      return
    }
    
    this.paymentService.getMoney({
      bankCardNumber: this.paymentForm.value.bankCardNumber.toString(),
      price: this.paymentForm.value.money
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        () => {
          this.toastr.success('Money was successfully withdrawn', 'Completed')
          this.dialogRef.close(true)
        },
        err => {
          this.toastr.error(err.error?.detail ?? 'Something went wrong', 'Failed')
          console.log(err); 
        }
      )
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
