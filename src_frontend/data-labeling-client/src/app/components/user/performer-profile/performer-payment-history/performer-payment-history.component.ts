import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { IJobPaymentHistory } from 'src/app/domain/responses/job-payment-history';
import { PaymentService } from '../../payment.service';

@Component({
  selector: 'app-performer-payment-history',
  templateUrl: './performer-payment-history.component.html',
  styleUrls: ['./performer-payment-history.component.css']
})
export class PerformerPaymentHistoryComponent implements OnInit, OnDestroy {
  paymentHistory: IJobPaymentHistory[] = []
  displayedColumns: string[] = ['bankCardNumber', 'price', 'createdData']
  dataSource = []
  loading = false

  private destroy$ = new Subject()

  constructor(private paymentService: PaymentService) { }

  ngOnInit(): void {
    this.getPayments()
  }

  getPayments() {
    this.loading = true
    this.paymentService.getPerformerHistory()
      .pipe(takeUntil(this.destroy$), finalize(() => this.loading = false))
      .subscribe(
        res => {
          this.paymentHistory = res.payload
          this.dataSource = this.paymentHistory.map((p, index) => {
            return { position: index + 1 ,...p}
          })
        },
        err => {
          console.log(err)
        }
      )
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
