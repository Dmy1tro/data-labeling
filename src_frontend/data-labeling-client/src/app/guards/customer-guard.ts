import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../components/authentication/auth.service";
import { FrontUrl } from "../domain/constants/front-url";

@Injectable({providedIn: 'root'})
export class CustomerGuard implements CanActivate {
    
    constructor(private authService: AuthService, private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authService.isCustomer) {
            return true
        } else {
            this.router.navigate(FrontUrl.defaultPage(this.authService.currentUser.userType))
            return false
        }
    }
}