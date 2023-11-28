import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { ResetPasswordForm } from "src/app/shared/models/reset-password.interface";
import { UserService } from "src/app/shared/services/user.service";
import { LoginDialogComponent } from "../login-dialog/login-dialog.component";

type NewType = UserService;

@Component({
  selector: "app-reset-password",
  templateUrl: "./reset-password.component.html",
  styleUrls: ["./reset-password.component.scss"],
})
export class ResetPasswordComponent {
  errors: string;
  isRequesting: boolean;
  submitted = false;
  form: ResetPasswordForm = {
    password: "",
    repeatPassword: "",
  };
  resetPasswordForm: FormGroup;
  responseOk: boolean;
  email: string;
  token: string;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    public dialog: MatDialog
  ) {
    this.email = this.route.snapshot.queryParamMap.get("email");
    this.token = this.route.snapshot.queryParamMap.get("token");
    this.initForm(this.form);
  }

  initForm(form: ResetPasswordForm) {
    this.resetPasswordForm = this.formBuilder.group({
      password: [form.password, Validators.required],
      repeatPassword: [form.repeatPassword, Validators.required],
    });
  }

  submit(form: FormGroup) {
    this.errors = null;
    this.responseOk = false;
    this.userService
      .sendResetPasswordForm(form.value, this.email, this.token)
      .subscribe({
        next: (_) => (this.responseOk = true),
        error: (error: any) => {
          this.errors = error.error;
        },
      });
  }

  openLoginDialog() {
    this.dialog.open(LoginDialogComponent);
  }
}
