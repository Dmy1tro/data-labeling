import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CustomerGuard } from "src/app/guards/customer-guard";
import { PerformerGuard } from "src/app/guards/performer-guard";
import { OrderResolver } from "src/app/resolvers/order.resolver";
import { CustomerOrdersComponent } from "./customer-orders/customer-orders.component";
import { PerformOrderComponent } from "./perform-order/perform-order.component";
import { PerformerOrdersComponent } from "./performer-orders/performer-orders.component";
import { ReviewOrderComponent } from "./review-order/review-order.component";

const routes: Routes = [
    { path: '', redirectTo: 'customer', pathMatch: 'full' },
    {
        path: 'customer',
        component: CustomerOrdersComponent,
        canActivate: [CustomerGuard]
    },
    {
        path: 'performer',
        component: PerformerOrdersComponent,
        canActivate: [PerformerGuard]
    },
    {
        path: 'performer/perform/:orderId',
        component: PerformOrderComponent,
        canActivate: [PerformerGuard],
    },
    {
        path: 'review/:orderId',
        component: ReviewOrderComponent,
        canActivate: [CustomerGuard],
        resolve: {
            order: OrderResolver
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OrderRoutingModule {}