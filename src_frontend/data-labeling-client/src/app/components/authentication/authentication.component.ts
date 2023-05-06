import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {

  displayLoginPage = false
  displayRegisterPage = false

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated) {
      this.router.navigate(FrontUrl.defaultPage(this.authService.currentUser.userType))
    }

    this.displayLoginPage = this.route.snapshot.routeConfig.path.includes('login')
    this.displayRegisterPage = this.route.snapshot.routeConfig.path.includes('register')
  }

}
