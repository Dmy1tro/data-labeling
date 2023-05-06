import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { AuthService } from "../components/authentication/auth.service";
import { FrontUrl } from "../domain/constants/front-url";

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
    
    constructor(private authService: AuthService, private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authService.isAuthenticated) {
            return true
        }
        
        this.router.navigate(FrontUrl.loginPage())
        return false
    }
}