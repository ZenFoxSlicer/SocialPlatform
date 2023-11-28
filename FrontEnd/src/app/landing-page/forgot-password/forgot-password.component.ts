import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ForgotPasswordForm } from "src/app/shared/models/forgot-password.interface";
import { UserService } from "src/app/shared/services/user.service";

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html",
  styleUrls: ["./forgot-password.component.scss"],
})
export class ForgotPasswordComponent {
  errors: string;
  isRequesting: boolean;
  submitted = false;
  form: ForgotPasswordForm = {
    userName: "",
    email: "",
  };
  forgotPasswordForm: FormGroup;
  responseOk: boolean;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    this.initForm(this.form);
  }

  initForm(form: ForgotPasswordForm) {
    this.forgotPasswordForm = this.formBuilder.group({
      userName: [form.userName, Validators.required],
      email: [form.email, Validators.required],
    });
  }

  submit(form: FormGroup) {
    this.errors = null;
    this.responseOk = false;
    const s = this.userService.sendForgotPasswordForm(form.value).subscribe({
      next: (_) => (this.responseOk = true),
      error: (error: any) => { debugger;this.errors = error.error.value},
    });
  }
}
