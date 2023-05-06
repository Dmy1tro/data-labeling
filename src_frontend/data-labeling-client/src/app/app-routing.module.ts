import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "./guards/auth-guard";

const routes: Routes = [
    { path: '', redirectTo: 'orders', pathMatch: 'full' },
    {
        path: 'orders',
        loadChildren: () => import('./components/order/order.module').then(m => m.OrderModule),
        canActivate: [AuthGuard]
    },
    { 
        path: 'authentication', 
        loadChildren: () => import('./components/authentication/authentication.module').then(m => m.AuthenticationModule)
    },
    {
        path: 'user',
        loadChildren: () => import('./components/user/user.module').then(m => m.UserModule)
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}