import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "../components/authentication/auth.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authService.isAuthenticated) {
            const user = this.authService.currentUser
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${user.token}`
                }
            })
        }

        return next.handle(req)
    }

}