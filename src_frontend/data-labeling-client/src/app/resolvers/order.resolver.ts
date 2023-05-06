import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  ActivatedRoute
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../components/authentication/auth.service';
import { OrderService } from '../components/order/order.service';
import { IApiResponse } from '../domain/responses/api-response';
import { IOrder } from '../domain/responses/order';

@Injectable({
  providedIn: 'root'
})
export class OrderResolver implements Resolve<IApiResponse<IOrder>> {

  constructor(private orderService: OrderService,
              private authService: AuthService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IApiResponse<IOrder>> {
    const orderId = +route.params.orderId    

    if (this.authService.isCustomer) {
      return this.orderService.getOrderForCustomer(orderId)
    }
    if (this.authService.isPerformer) {
      return this.orderService.getOrderForPerformer(orderId)
    }

    return of({payload: null});
  }
}
