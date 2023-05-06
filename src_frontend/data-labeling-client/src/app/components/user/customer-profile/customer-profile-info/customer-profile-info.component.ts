import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/components/authentication/auth.service';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { ICustomerStatistic } from 'src/app/domain/responses/customer-statistic';
import { IUserData } from 'src/app/domain/responses/user-data';
import { toUserType } from 'src/app/shared/helpers/name-helper';
import { UserService } from '../../user.service';

@Component({
  selector: 'app-customer-profile-info',
  templateUrl: './customer-profile-info.component.html',
  styleUrls: ['./customer-profile-info.component.css']
})
export class CustomerProfileInfoComponent implements OnInit, OnDestroy {

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
