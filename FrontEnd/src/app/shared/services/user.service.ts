import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { ConfigService } from "./config.service";

import { BaseService } from "./base.service";

import { BehaviorSubject } from "rxjs/internal/BehaviorSubject";
import { Observable } from "rxjs";
import { Credentials } from "../models/credentials.interface";
import { LoginResult } from "../models/LoginResult";
import { Router } from "@angular/router";
import { RegistrationForm } from "../models/registration-form.interface";
import { ForgotPasswordForm } from "../models/forgot-password.interface";
import { ResetPasswordForm } from "../models/reset-password.interface";
import { Publication } from "../models/publication.interface";
import jwtDecode from "jwt-decode";
import { JwtPayload } from "jwt-decode";
import { UserInfo } from "../models/user-info.interface";

@Injectable()
export class UserService extends BaseService {
  baseUrl = "";

  // Observable navItem source
  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this.authNavStatusSource.asObservable();

  private loggedIn = false;
  decodedToken: JwtPayload;

  constructor(
    private http: HttpClient,
    configService: ConfigService,
    private router: Router
  ) {
    super();
    this.loggedIn = !!localStorage.getItem("auth_token");
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this.authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
    if (this.loggedIn) {
      this.decodedToken = this.decodeToken(localStorage.getItem("auth_token"));
      console.log(this.decodedToken);
    }
  }

  login(user: Credentials): Observable<LoginResult> {
    return this.http.post(this.baseUrl + "/auth/login", {
      username: user.userName,
      password: user.password,
    }) as Observable<LoginResult>;
  }

  sendForgotPasswordForm(value: ForgotPasswordForm): Observable<any> {
    return this.http.post(this.baseUrl + "/auth/forgot-password", {
      username: value.userName,
      email: value.email,
    }) as Observable<any>;
  }

  saveUserData(info: UserInfo): Observable<LoginResult> {
    return this.http.post(
      this.baseUrl + "/auth/save-user-info",
      info
    ) as Observable<LoginResult>;
  }

  sendResetPasswordForm(
    value: ResetPasswordForm,
    email: string,
    token: string
  ): Observable<any> {
    return this.http.post(this.baseUrl + "/auth/reset-password", {
      password: value.password,
      confirmPassword: value.repeatPassword,
      email,
      token,
    }) as Observable<any>;
  }

  register(form: RegistrationForm): Observable<any> {
    return this.http.post(this.baseUrl + "/register", {
      firstName: form.firstName,
      lastName: form.lastName,
      userName: form.userName,
      email: form.email,
      password: form.password,
      repeatPassword: form.repeatPassword,
    }) as Observable<any>;
  }

  logout() {
    localStorage.removeItem("auth_token");
    this.loggedIn = false;
    this.authNavStatusSource.next(false);
    this.router.navigate(["landing-page"], { replaceUrl: true });
  }

  getBasicInfo(userName: string): Observable<UserInfo> {
    return this.http.get(
      this.baseUrl + `/auth/basic-user-info/${userName}`
    ) as Observable<UserInfo>;
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  getUserName() {
    return this.isLoggedIn() ? this.decodedToken["sub"] : "";
  }

  getUserId() {
    return this.decodedToken["id"];
  }

  public decodeToken(token: string): JwtPayload {
    return jwtDecode(token) as JwtPayload;
  }
}
