import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { OrderType } from 'src/app/domain/enums/order-type';
import { IOrder } from 'src/app/domain/responses/order';
import { PerformDataInfoComponent } from 'src/app/shared/components/perform-data-info/perform-data-info.component';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-perform-order',
  templateUrl: './perform-order.component.html',
  styleUrls: ['./perform-order.component.css']
})
export class PerformOrderComponent implements OnInit, OnDestroy {

  order: IOrder

  private destroy$ = new Subject()

  constructor(private route: ActivatedRoute,
              private orderService: OrderService,
              private toastr: ToastrService,
              private router: Router,
              private matDialog: MatDialog) { }

  ngOnInit(): void {
    const orderId = +this.route.snapshot.params.orderId
    this.getOrder(orderId)
  }

  getOrder(id: number) {
    this.orderService.getOrderForPerformer(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.order = res.payload
        },
        err => {
          this.toastr.error(err.error.detail, 'Error')
          console.log(err);
        }
      )
  }

  back() {
    this.router.navigate(FrontUrl.ordersForPerformerPage())
  }

  showHelp() {
    this.matDialog.open(PerformDataInfoComponent, 
      { 
        width: '35%', 
        autoFocus: true, 
        maxHeight: '95vh', 
        data: { isLabel: true, order: this.order } 
      })
  }

  onOrderChanged() {
    this.getOrder(this.order.id)
  }

  get IsCollectDataOrder(): boolean {
    return this.order.type === OrderType.CollectData
  }

  get isLabelDataOrder(): boolean {
    return this.order.type === OrderType.LabelData
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
