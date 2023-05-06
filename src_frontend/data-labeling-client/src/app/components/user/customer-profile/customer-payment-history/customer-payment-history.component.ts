import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { IOrderPaymentHistory } from 'src/app/domain/responses/order-payment-history';
import { PaymentService } from '../../payment.service';

@Component({
  selector: 'app-customer-payment-history',
  templateUrl: './customer-payment-history.component.html',
  styleUrls: ['./customer-payment-history.component.css']
})
export class CustomerPaymentHistoryComponent implements OnInit, OnDestroy {
  paymentHistory: IOrderPaymentHistory[] = []
  displayedColumns: string[] = ['order', 'price', 'createdData']
  dataSource = []
  loading = false

  private destroy$ = new Subject()

  constructor(private paymentService: PaymentService) { }

  ngOnInit(): void {
    this.getPaymentHistory()
  }

  getPaymentHistory() {
    this.loading = true
    this.paymentService.getCustomerHistory()
      .pipe(takeUntil(this.destroy$), finalize(() => this.loading = false))
      .subscribe(
        res => {
          this.paymentHistory = res.payload
          this.dataSource = this.paymentHistory.map((p, i) => {
            return { position: i + 1, ...p }
          })
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
