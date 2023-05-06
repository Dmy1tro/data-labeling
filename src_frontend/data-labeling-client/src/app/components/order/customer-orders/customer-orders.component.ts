import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { OrderPriority } from 'src/app/domain/enums/order-priority';
import { OrderType } from 'src/app/domain/enums/order-type';
import { IOrder } from 'src/app/domain/responses/order';
import { ConfirmModalComponent } from 'src/app/shared/components/confirm-modal/confirm-modal.component';
import { enumSelector } from 'src/app/shared/helpers/enum-selector';
import { toPriorityTitle, toTypeTitle, toYesNo } from 'src/app/shared/helpers/name-helper';
import { CreateOrderComponent } from '../create-order/create-order.component';
import { DataService } from '../data.service';
import { OrderForCustomerFilter, OrderService } from '../order.service';
import { PayForOrderComponent } from '../pay-for-order/pay-for-order.component';

@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.css']
})
export class CustomerOrdersComponent implements OnInit, OnDestroy {

  orders: IOrder[] = []
  filteredOrders = this.orders
  filterForm: FormGroup
  orderTypes = enumSelector(OrderType)
  orderPriorities = enumSelector(OrderPriority)
  sortAsc = true
  orderProperty = ''

  private destroy$ = new Subject<void>()

  constructor(private orderService: OrderService,
              private dataService: DataService,
              private toastr: ToastrService,
              private fb: FormBuilder,
              private dialog: MatDialog,
              private router: Router) { }

  ngOnInit(): void {
    this.createFilterForm()
    this.getOrders()
    this.orderTypes = enumSelector(OrderType)
    this.orderPriorities = enumSelector(OrderPriority)
  }

  // TODO: implement reviewing order

  getOrders() {
    this.orderService.getForCustomer(new OrderForCustomerFilter())
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => {
          this.orders = res.payload
          this.applyFilter()
          this.sortBy(this.orderProperty)
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
      maxPriority: [null],
      isCompleted: [null],
      isCanceled: [null]
    })
  }

  add() {
    this.dialog.open(CreateOrderComponent, { width: '35%', autoFocus: true, maxHeight: '96vh' })
      .afterClosed()
      .pipe(
        takeUntil(this.destroy$),
        filter(res => !!res))
      .subscribe((res) =>{
        if (!!res) this.getOrders()
      })
  }

  payForOrder(orderId: number) {
    this.dialog.open(PayForOrderComponent, { autoFocus: true, data: orderId })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (!!res) this.refreshOrder(orderId)
      })
  }

  downloadDataSet(orderId: number) {
    this.dataService.downloadDataSet(orderId)
  }

  reviewOrder(orderId: number) {    
    this.router.navigate(FrontUrl.reviewOrder(orderId))
  }

  refreshOrder(orderId: number) {
   this.orderService.getOrderForCustomer(orderId)
    .pipe(takeUntil(this.destroy$))
    .subscribe(
      res => {
        const index = this.orders.findIndex(o => o.id === res.payload.id)
        this.orders[index] = res.payload
        this.toastr.success('Refreshed', `${res.payload.name}`)
      }
    ) 
  }

  toPriorityTitle = toPriorityTitle
  toTypeTitle = toTypeTitle
  toYesNo = toYesNo

  cancelOrder(order: IOrder) {
    this.dialog.open(ConfirmModalComponent, { width: 'auto', autoFocus: true, data: `Are you sure you want to cancel order '${order.name}' ?` })
      .afterClosed()
      .pipe(
        takeUntil(this.destroy$),
        filter(res => !!res),
        switchMap(res => {
          return this.orderService.cancel(order.id)
        }))
      .subscribe(() => {
        this.getOrders()
      })
  }

  sortBy(property: string) {
    if (property === 'name') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.name > b.name : a.name < b.name) ? 1 : -1)
    }

    if (property === 'progress') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.currentProgress / a.datSetRequiredCount > b.currentProgress / b.datSetRequiredCount : a.currentProgress / a.datSetRequiredCount < b.currentProgress / b.datSetRequiredCount) ? 1 : -1)
    }

    if (property === 'requirements') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.requirements > b.requirements : a.requirements < b.requirements) ? 1 : -1)
    }

    if (property === 'price') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.price > b.price : a.price < b.price) ? 1 : -1)
    }

    if (property === 'type') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.type > b.type : a.type < b.type) ? 1 : -1)
    }

    if (property === 'priority') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.priority > b.priority : a.priority < b.priority) ? 1 : -1)
    }

    if (property === 'isCompleted') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.isCompleted > b.isCompleted : a.isCompleted < b.isCompleted) ? 1 : -1)
    }

    if (property === 'isCanceled') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.isCanceled > b.isCanceled : a.isCanceled < b.isCanceled) ? 1 : -1)
    }

    if (property === 'deadline') {
      this.filteredOrders = this.filteredOrders.sort((a, b) => (this.sortAsc ? a.deadline > b.deadline : a.deadline < b.deadline) ? 1 : -1)
    }

    if (property) {
      this.orderProperty = property
      this.sortAsc = !this.sortAsc
    }
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
    
    if (formValue.isCompleted != null) {
      this.filteredOrders = this.filteredOrders.filter(o => o.isCompleted === formValue.isCompleted)
    }

    if (formValue.isCanceled != null) {            
      this.filteredOrders = this.filteredOrders.filter(o => o.isCanceled === formValue.isCanceled)
    }
  }

  resetFilter() {
    this.filterForm.reset()
    this.filteredOrders = this.orders
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
