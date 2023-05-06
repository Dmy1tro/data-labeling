import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { FrontUrl } from 'src/app/domain/constants/front-url';
import { UserType } from 'src/app/domain/enums/user-type';
import { enumSelector } from 'src/app/shared/helpers/enum-selector';
import { FormValidationService } from 'src/app/shared/services/error-message.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit, OnDestroy {

  loginForm: FormGroup
  hidePassword = true
  userTypes = enumSelector(UserType)
  loading = false

  private destroy$ = new Subject<void>()

  constructor(private authService: AuthService,
              private fb: FormBuilder,
              private router: Router,
              private toastr: ToastrService,
              public formValidationService: FormValidationService) { }

  ngOnInit(): void {
    this.createForm()
  }

  onSubmit() {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched()
      return
    }
    this.loading = true
    this.authService.login(this.loginForm.value)
      .pipe(takeUntil(this.destroy$),finalize(() => this.loading = false))
      .subscribe(
        () => {
          this.router.navigate(FrontUrl.defaultPage(this.authService.currentUser.userType))
        },
        (err) => {
          this.toastr.error(err.error.detail, 'Error')
          console.log(err);
        }
      )
  }

  private createForm() {
    this.loginForm = this.fb.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required]],
      userType: [UserType.Performer, [Validators.required]]
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }

}
