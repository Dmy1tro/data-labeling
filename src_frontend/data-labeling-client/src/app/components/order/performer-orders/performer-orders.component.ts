import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { OrderPriority } from 'src/app/domain/enums/order-priority';
import { OrderType } from 'src/app/domain/enums/order-type';
import { IOrder } from 'src/app/domain/responses/order';
import { enumSelector, IEnumSelect } from 'src/app/shared/helpers/enum-selector';
import { toPriorityTitle, toTypeTitle } from 'src/app/shared/helpers/name-helper';
import { OrderForPerformerFilter, OrderService } from '../order.service';

@Component({
  selector: 'app-performer-orders',
  templateUrl: './performer-orders.component.html',
  styleUrls: ['./performer-orders.component.css']
})
export class PerformerOrdersComponent implements OnInit, OnDestroy {

  orders: IOrder[] = []
  filteredOrders: IOrder[] = []
  filterForm: FormGroup
  orderTypes = enumSelector(OrderType)
  orderPriorities = enumSelector(OrderPriority)

  private destroy$ = new Subject<void>()
  
  constructor(private orderService: OrderService,
              private toastr: ToastrService,
              private fb: FormBuilder,
              private router: Router) { }

  ngOnInit(): void {
    this.createFilterForm()
    this.getOrders()
    this.getAllowedPriorities()
  }

  getOrders() {
    this.orderService.getForPerformer(new OrderForPerformerFilter())
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.orders = res.payload
          this.applyFilter()
        },
        err => {
          this.toastr.error(err.error.detail, 'Error')
          console.log(err);
        }
      )
  }

  getAllowedPriorities() {
    this.orderService.getAllowedPriorities()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.orderPriorities = this.orderPriorities.filter(p => res.payload.some(r => +r === p.value))
        },
        err => {
          this.toastr.error(err.error.detail, 'Error')
          console.log(err);
        }
      )
  }

  createFilterForm() {
    this.filterForm = this.fb.group({
      orderType: [null],
      minPriority: [null],
      maxPriority: [null]
    })
  }

  applyFilter() {
    const formValue = this.filterForm.value
    this.filteredOrders = this.orders

    if (!!formValue.orderType) {
      this.filteredOrders = this.filteredOrders.filter(o => o.type === formValue.orderType)
    }

    if (!!formValue.minPriority) {
      this.filteredOrders = this.filteredOrders.filter(o => o.priority >= formValue.minPriority)
    }    

    if (!!formValue.maxPriority) {
      this.filteredOrders = this.filteredOrders.filter(o => o.priority <= formValue.maxPriority)
    }
  }

  performOrder(orderId: number) {
    this.router.navigate(FrontUrl.performOrderPage(orderId))
  }

  
  toPriorityTitle = toPriorityTitle
  toTypeTitle = toTypeTitle

  resetFilter() {
    this.filterForm.reset()
    this.filteredOrders = this.orders
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }

}
