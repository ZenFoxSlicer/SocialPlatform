import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Credentials } from '../../shared/models/credentials.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from './../../shared/services/user.service';
import { Subscription } from 'rxjs';
import { setAuthToken } from '../../helpers/storage-helper';
import { MatDialogRef } from '@angular/material/dialog';



@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.scss']
})
export class LoginDialogComponent implements OnInit {

  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted = false;
  credentials: Credentials = { userName: '', password: '' };
  loginForm: FormGroup;

  constructor(
    private userService: UserService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<LoginDialogComponent>
  ) { }

  ngOnInit() {

    this.initForm(this.credentials);
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param.brandNew;
        this.credentials.userName = param.userName;
      });
  }

  initForm(credentials: Credentials) {
    this.loginForm = this.formBuilder.group({
      userName: [credentials.userName, Validators.required],
      password: [credentials.password, Validators.required],
    });
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  login(form: FormGroup) {
    const s = this.userService.login(form.value).subscribe((data) => {
      const remind = form.value.remind;
      setAuthToken(data.authToken, true);
      this.router.navigate(['home'], { replaceUrl: true });
      this.dialogRef.close();
    }, (err) => {
      this.errors = err.error.value;
    });
  }

  forgotPassword(){
    this.router.navigate(['forgot-password'], { replaceUrl: true });
    this.dialogRef.close();
  }

  close(){
    this.dialogRef.close();
  }

}
