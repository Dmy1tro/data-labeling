import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiUrl } from 'src/app/domain/constants/api-url';
import { IApiResponse } from 'src/app/domain/responses/api-response';
import { IJobPaymentHistory } from 'src/app/domain/responses/job-payment-history';
import { ILiqPayData } from 'src/app/domain/responses/liqpay-data';
import { IOrderPaymentHistory } from 'src/app/domain/responses/order-payment-history';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private httpClient: HttpClient) { }

  getMoney(data): Observable<any> {
    const uri = ApiUrl.getMoney()
    return this.httpClient.post<any>(uri, data)
  }

  confirmPay(orderId: number): Observable<any> {
    const uri = ApiUrl.confirmPay(orderId)
    return this.httpClient.post(uri, null)
  }

  getCustomerHistory(): Observable<IApiResponse<any>> {
    const uri = ApiUrl.getCustomerHistory()
    return this.httpClient.get<IApiResponse<any>>(uri)
  }

  getPerformerHistory(): Observable<IApiResponse<IJobPaymentHistory[]>> {
    const uri = ApiUrl.getPerformerHistory()
    return this.httpClient.get<IApiResponse<IJobPaymentHistory[]>>(uri)
  }

  getLiqPayData(orderId: number): Observable<IApiResponse<ILiqPayData>> {
    const uri = ApiUrl.getLiqPayData(orderId)
    return this.httpClient.get<IApiResponse<ILiqPayData>>(uri)
  }
}
