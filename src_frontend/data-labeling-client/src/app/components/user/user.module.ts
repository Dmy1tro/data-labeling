import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { PerformerProfileComponent } from './performer-profile/performer-profile.component';
import { CustomerProfileComponent } from './customer-profile/customer-profile.component';
import { GetMoneyComponent } from './performer-profile/get-money/get-money.component';
import { ProfileInfoComponent } from './performer-profile/profile-info/profile-info.component';
import { CustomerProfileInfoComponent } from './customer-profile/customer-profile-info/customer-profile-info.component';
import { CustomerPaymentHistoryComponent } from './customer-profile/customer-payment-history/customer-payment-history.component';
import { PerformerPaymentHistoryComponent } from './performer-profile/performer-payment-history/performer-payment-history.component';


@NgModule({
  declarations: [
    PerformerProfileComponent,
    CustomerProfileComponent,
    GetMoneyComponent,
    ProfileInfoComponent,
    CustomerProfileInfoComponent,
    CustomerPaymentHistoryComponent,
    PerformerPaymentHistoryComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule
  ]
})
export class UserModule { }
