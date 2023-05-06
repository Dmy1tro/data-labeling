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
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  registerForm: FormGroup
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
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched()
      return
    }
    this.loading = true
    this.authService.register(this.registerForm.value)
      .pipe(takeUntil(this.destroy$), finalize(() => this.loading = false))
      .subscribe(() => {
        const email = this.registerForm.get('email').value
        this.toastr.success(`Confirmation message was sent to ${email}`)
        this.router.navigate(FrontUrl.loginPage())
      },
      err => {
        this.toastr.error(err.error.detail, 'Error')
        console.log(err);
      })
  }

  private createForm() {
    this.registerForm = this.fb.group({
      email: [null, [Validators.required, Validators.email]],
      firstName: [null, [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      lastName: [null, [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      userType: [UserType.Performer, [Validators.required]],
      password: [null, [Validators.required]]
    })
  }

  ngOnDestroy() {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
