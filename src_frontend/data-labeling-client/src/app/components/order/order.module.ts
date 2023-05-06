import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderRoutingModule } from './order-routing.module';
import { CustomerOrdersComponent } from './customer-orders/customer-orders.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CreateOrderComponent } from './create-order/create-order.component';
import { PerformerOrdersComponent } from './performer-orders/performer-orders.component';
import { PerformOrderComponent } from './perform-order/perform-order.component';
import { PerformLabelDataComponent } from './perform-order/perform-label-data/perform-label-data.component';
import { PerformCollectDataComponent } from './perform-order/perform-collect-data/perform-collect-data.component';
import { PayForOrderComponent } from './pay-for-order/pay-for-order.component';
import { ReviewOrderComponent } from './review-order/review-order.component';
import { ImageDrawingCustomModule } from '../external/image-drawing-custom/image-drawing-custom.module';



@NgModule({
  declarations: [
    CustomerOrdersComponent,
    CreateOrderComponent,
    PerformerOrdersComponent,
    PerformOrderComponent,
    PerformLabelDataComponent,
    PerformCollectDataComponent,
    PayForOrderComponent,
    ReviewOrderComponent
  ],
  imports: [
    CommonModule,
    OrderRoutingModule,
    ImageDrawingCustomModule,
    SharedModule
  ]
})
export class OrderModule { }
