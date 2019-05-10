import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Credentials } from '../../shared/services/models/credentials.interface';
import { UserService } from '../../shared/services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { setAuthToken } from 'src/app/helpers/storage-helper';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted = false;
  credentials: Credentials = { email: '', password: '' };
  loginForm: FormGroup;

  constructor(
    private userService: UserService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder
    ) { }

  ngOnInit() {

    this.initForm(this.credentials);
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param.brandNew;
        this.credentials.email = param.email;
      });
  }

  initForm(credentials: Credentials) {
    this.loginForm = this.formBuilder.group({
      email: [credentials.email, Validators.required],
      password: [credentials.password, Validators.required],
    });
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  login(form: FormGroup ) {

    const s = this.userService.login(form.value).subscribe(data => {
      const remind = form.value.remind;

      setAuthToken(data.authToken, true);
      this.router.navigate([this.activatedRoute.snapshot.queryParams.returnUrl || '']);
  }, err => {
    this.errors = err.error.value;
  });
    // this.submitted = true;
    // this.isRequesting = true;
    // this.errors = '';
    // if (form.valid) {
    //   const res = this.userService.login(form.value.email, form.value.password);
    //   if (res) {
    //     this.router.navigate(['/home']);
    //   }
    // }
  }
}


