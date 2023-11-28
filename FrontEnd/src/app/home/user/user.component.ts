import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { setAuthToken } from "src/app/helpers/storage-helper";
import { LoginResult } from "src/app/shared/models/LoginResult";
import { UserInfo } from "src/app/shared/models/user-info.interface";
import { UserService } from "src/app/shared/services/user.service";

@Component({
  templateUrl: "./user.component.html",
  styleUrls: ["./user.component.scss"],
})
export class UserComponent implements OnInit {
  errors: Array<string>;
  isRequesting: boolean;
  submitted = false;
  info: UserInfo = {
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    phoneNumber: "",
  } as UserInfo;
  userForm: FormGroup;
  success: boolean;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {}
  ngOnInit(): void {
    this.initForm(this.info);
    this.userService
      .getBasicInfo(this.userService.getUserName())
      .subscribe((response: UserInfo) => {
        this.info = response;
        this.userForm.patchValue(response);
      });
  }

  initForm(info: UserInfo) {
    this.userForm = this.formBuilder.group({
      firstName: [info.firstName, Validators.required],
      lastName: [info.lastName, Validators.required],
      userName: [info.userName, Validators.required],
      email: [info.email, Validators.required],
      phoneNumber: [info.phoneNumber, Validators.required],
    });
  }

  saveData() {
    this.success = false;
    var value = this.userForm.getRawValue() as UserInfo;
    value.id = this.info.id;
    this.userService.saveUserData(value).subscribe((result: LoginResult) => {
      setAuthToken(result.authToken, true);
      this.success = true;
    });
  }
}
