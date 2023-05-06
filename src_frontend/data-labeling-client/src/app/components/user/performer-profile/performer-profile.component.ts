import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { IPerformer } from 'src/app/domain/responses/performer';
import { IPerformerStatistic } from 'src/app/domain/responses/performer-statistic';
import { toUserRating, toUserType } from 'src/app/shared/helpers/name-helper';
import { UserService } from '../user.service';
import { GetMoneyComponent } from './get-money/get-money.component';

@Component({
  selector: 'app-performer-profile',
  templateUrl: './performer-profile.component.html',
  styleUrls: ['./performer-profile.component.css']
})
export class PerformerProfileComponent implements OnInit, OnDestroy {

  performer: IPerformer
  statistic: IPerformerStatistic

  private destroy$ = new Subject()

  constructor(private userService: UserService,
              private router: Router,
              private matDialog: MatDialog) { }

  ngOnInit(): void {
    this.getPerformer()
    this.getStatistic()
  }

  getPerformer() {
    this.userService.getPerformerInfo()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => this.performer = res.payload,
        err => console.log(err)
      )
  }

  getStatistic() {
    this.userService.getPerformerStatistic()
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        res => this.statistic = res.payload,
        err => console.log(err)
      )
  }

  getMoney() {
    this.matDialog.open(GetMoneyComponent, { width: '35%', autoFocus: true })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        if (!!res) this.getPerformer()
      })
  }

  goBack() {
    this.router.navigate(FrontUrl.ordersForPerformerPage())
  }

  toUserType = toUserType
  toUserRating = toUserRating

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
