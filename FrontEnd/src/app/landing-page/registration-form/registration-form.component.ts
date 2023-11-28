import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { setAuthToken } from 'src/app/helpers/storage-helper';
import { RegistrationForm } from 'src/app/shared/models/registration-form.interface';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {

  private subscription: Subscription;

  brandNew: boolean;
  errors: Array<string>;
  isRequesting: boolean;
  submitted = false;
  credentials: RegistrationForm =
  {
    firstName: '',
    lastName:'',
    email: '',
    password: '',
    repeatPassword:'',
    userName:''
  }
  registerForm: FormGroup;
  success: boolean;

  constructor(
    private userService: UserService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<RegistrationFormComponent>
  ) { }

  ngOnInit() {

    this.initForm(this.credentials);
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param.brandNew;
        this.credentials.email = param.email;
      });
  }

  initForm(credentials: RegistrationForm) {
    this.registerForm = this.formBuilder.group({
      firstName: [credentials.firstName, Validators.required],
      lastName: [credentials.lastName, Validators.required],
      email: [credentials.email, Validators.required],
      password: [credentials.password, Validators.required],
      repeatPassword: [credentials.repeatPassword, Validators.required],
      userName: [credentials.userName, Validators.required],
    });
  }

  ngOnDestroy() {
    // prevent memory leak by unsubscribing
    this.subscription.unsubscribe();
  }

  close(){
    this.dialogRef.close();
  }

  register(form: FormGroup) {
    this.errors = [];
    const s = this.userService.register(form.value).subscribe(_ => {
      this.success = true;
    }, err => {
      debugger
      this.errors = err.error.Error;
    });
  }
}
