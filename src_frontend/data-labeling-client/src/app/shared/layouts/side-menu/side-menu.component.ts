import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, shareReplay, takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/components/authentication/auth.service';
import { FrontUrl } from 'src/app/domain/constants/front-url';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.css']
})
export class SideMenuComponent implements OnInit, OnDestroy {


  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  translateOn: boolean;
  opened$ = false;

  private destroy$ = new Subject<void>();

  constructor(private breakpointObserver: BreakpointObserver,
              private authService: AuthService,
              private router: Router) { }

  ngOnInit(): void {
    this.opened$ = this.authService.isAuthenticated
    this.authService.userObservable
      .pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.opened$ = res != null
      })
  }

  goToOrders() {
    this.router.navigate(FrontUrl.ordersPage(this.authService.currentUser.userType))
  }

  goToProfile() {
    this.router.navigate(FrontUrl.profile(this.authService.currentUser.userType))
  }

  logout(): void {
    this.authService.logout()
    this.router.navigate(FrontUrl.loginPage())
  }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated
  }

  get isCustomer(): boolean {
    return this.authService.isCustomer
  }

  get isPerformer(): boolean {
    return this.authService.isPerformer
  }
  
  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }

}
