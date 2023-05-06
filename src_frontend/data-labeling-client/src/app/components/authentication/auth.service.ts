import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {tap} from 'rxjs/operators';
import { ApiUrl } from 'src/app/domain/constants/api-url';
import { UserRole } from 'src/app/domain/constants/user-role';
import { IApiResponse } from 'src/app/domain/responses/api-response';
import { IUserData } from 'src/app/domain/responses/user-data';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userObservable: Observable<IUserData>
  private currentUserSubject: BehaviorSubject<IUserData>

  constructor(private httpClient: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<IUserData>(this.getUserData())
    this.userObservable = this.currentUserSubject.asObservable()
  }

  get isAuthenticated(): boolean {
    return this.currentUserSubject.value != null
  }

  get currentUser(): IUserData {
    return this.currentUserSubject.value
  }

  get isPerformer(): boolean {
    return this.isAuthenticated && this.currentUser.roles.includes(UserRole.Performer)
  }

  get isCustomer(): boolean {
    return this.isAuthenticated && this.currentUser.roles.includes(UserRole.Customer)
  }

  login(data): Observable<IApiResponse<IUserData>> {
    const uri = ApiUrl.login()
    return this.httpClient.post<IApiResponse<IUserData>>(uri, data)
      .pipe(
        tap(res => {
          this.setUserData(res.payload)
          this.currentUserSubject.next(res.payload)
        })
      )
  }

  register(data): Observable<IApiResponse<IUserData>> {
    const uri = ApiUrl.register()
    return this.httpClient.post<IApiResponse<IUserData>>(uri, data)
  }

  logout(): void {
    this.removeUserData()
    this.currentUserSubject.next(null)
  }

  private getUserData(): IUserData {
    return JSON.parse(localStorage.getItem('user-data')) as IUserData
  }

  private setUserData(userData: IUserData) {
    localStorage.setItem('user-data', JSON.stringify(userData))
  }

  private removeUserData() {
    localStorage.removeItem('user-data')
  }
}
