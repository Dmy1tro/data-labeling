import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiUrl } from 'src/app/domain/constants/api-url';
import { IApiResponse } from 'src/app/domain/responses/api-response';
import { IData } from 'src/app/domain/responses/data';
import { IDataWithImageResponse as IDataWithImageResponse } from 'src/app/domain/responses/image-response';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private httpClient: HttpClient) { }

  uploadRawData(data): Observable<any> {
    const uri = ApiUrl.uploadRawData()
    return this.httpClient.post<Observable<any>>(uri, data)
  }

  labelData(data): Observable<any> {
    const uri = ApiUrl.labelData()
    return this.httpClient.post<any>(uri, data)
  }

  getDataForLabeling(orderId: number): Observable<IApiResponse<IDataWithImageResponse>> {
    const uri = ApiUrl.getDataForLabeling(orderId)
    return this.httpClient.get<IApiResponse<IDataWithImageResponse>>(uri)
  }

  getCompletedDataset(orderId: number): Observable<IApiResponse<IData>> {
    const uri = ApiUrl.getCompletedDataSet(orderId, 0, 10000, '');
    return this.httpClient.get<IApiResponse<IData>>(uri)
  }

  downloadDataSet(orderId: number) {
    const uri = ApiUrl.downloadDataSet(orderId)
    return this.httpClient.get(uri, {responseType: 'blob'})
      .subscribe(file => {
        saveAs(file, `${orderId}_Order.zip`)
      })
  }

  getDataSetForReview(orderId: number, performerId: number): Observable<IApiResponse<IDataWithImageResponse[]>> {
    const uri = ApiUrl.getDatasetForReview(orderId, performerId)
    return this.httpClient.get<IApiResponse<IDataWithImageResponse[]>>(uri)
  }
}
