import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiUrl } from 'src/app/domain/constants/api-url';
import { IApiResponse } from 'src/app/domain/responses/api-response';
import { ICustomerStatistic } from 'src/app/domain/responses/customer-statistic';
import { IPerformer } from 'src/app/domain/responses/performer';
import { IPerformerStatistic } from 'src/app/domain/responses/performer-statistic';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getPerformerInfo(): Observable<IApiResponse<IPerformer>> {
    const uri = ApiUrl.getPerformerInfo()
    return this.httpClient.get<IApiResponse<IPerformer>>(uri)
  }

  getCustomerStatistic(): Observable<IApiResponse<ICustomerStatistic>> {
    const uri = ApiUrl.getCustomerStatistic()
    return this.httpClient.get<IApiResponse<ICustomerStatistic>>(uri)
  }

  getPerformerStatistic(): Observable<IApiResponse<IPerformerStatistic>> {
    const uri = ApiUrl.getPerformerStatistic()
    return this.httpClient.get<IApiResponse<IPerformerStatistic>>(uri)
  }
}
