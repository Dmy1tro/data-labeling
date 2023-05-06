import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerGuard } from 'src/app/guards/customer-guard';
import { PerformerGuard } from 'src/app/guards/performer-guard';
import { CustomerProfileComponent } from './customer-profile/customer-profile.component';
import { PerformerProfileComponent } from './performer-profile/performer-profile.component';

const routes: Routes = [
  { path: '', redirectTo: 'customer', pathMatch: 'full' },
  {
    path: 'customer', component: CustomerProfileComponent, canActivate: [CustomerGuard]
  },
  {
    path: 'performer', component: PerformerProfileComponent, canActivate: [PerformerGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
