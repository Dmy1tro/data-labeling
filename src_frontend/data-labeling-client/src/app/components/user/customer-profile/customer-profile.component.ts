import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { ICustomerStatistic } from 'src/app/domain/responses/customer-statistic';
import { IUserData } from 'src/app/domain/responses/user-data';
import { toUserType } from 'src/app/shared/helpers/name-helper';
import { AuthService } from '../../authentication/auth.service';
import { UserService } from '../user.service';

@Component({
  selector: 'app-customer-profile',
  templateUrl: './customer-profile.component.html',
  styleUrls: ['./customer-profile.component.css']
})
export class CustomerProfileComponent implements OnInit, OnDestroy {

  userData: IUserData
  statistic: ICustomerStatistic

  private destroy$ = new Subject()

  constructor(private authService: AuthService,
              private userService: UserService,
              private router: Router) { }

  ngOnInit(): void {
    this.userData = this.authService.currentUser
    this.getStatistic()
  }

  getStatistic() {
    this.userService.getCustomerStatistic()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => this.statistic = res.payload,
        err => console.log(err)
      )
  }

  goBack() {
    this.router.navigate(FrontUrl.orderForCustomerPage())
  }

  toUserType = toUserType

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
