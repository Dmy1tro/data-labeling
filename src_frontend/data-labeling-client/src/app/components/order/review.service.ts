import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiUrl } from 'src/app/domain/constants/api-url';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private httpClient: HttpClient) { }

  addReview(data) {
    const uri = ApiUrl.addReview()
    return this.httpClient.post(uri, data)
  }
}
