import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ILiqPayData } from 'src/app/domain/responses/liqpay-data';
import { IOrder } from 'src/app/domain/responses/order';
import { PaymentService } from '../../user/payment.service';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-pay-for-order',
  templateUrl: './pay-for-order.component.html',
  styleUrls: ['./pay-for-order.component.css']
})
export class PayForOrderComponent implements OnInit, OnDestroy {

  order: IOrder
  liqPayData: ILiqPayData
  liqPayUrl: string

  private destroy$ = new Subject()

  constructor(private dialogRef: MatDialogRef<PayForOrderComponent>,
              @Inject(MAT_DIALOG_DATA) private orderId: number,
              private orderService: OrderService,
              private paymentService: PaymentService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getOrder()
    this.getLiqPayData()
  }

  getOrder() {
    this.orderService.getOrderForCustomer(this.orderId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.order = res.payload
        },
        err => console.log(err)
      )
  }

  private getLiqPayData() {
    this.paymentService.getLiqPayData(this.orderId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.liqPayData = res.payload
          this.liqPayUrl = `https://www.liqpay.ua/api/3/checkout?data=${this.liqPayData.dataHash}&signature=${this.liqPayData.signatureHash}`
        },
        err => {
          console.log(err)
        }
      )
  }

  onSubmit() {
    this.paymentService.confirmPay(this.orderId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.toastr.success('Order has been paid')
          this.dialogRef.close(true)
        },
        err => {
          console.log(err);
        }
      )
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
