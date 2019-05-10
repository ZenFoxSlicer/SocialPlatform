import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpParams } from '@angular/common/http';

import { UserRegistration } from './models/user-registration.interface';
import { ConfigService } from './config.service';

import { BaseService } from './base.service';

import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs';
import { Credentials } from './models/credentials.interface';
import { LoginResult } from './models/LoginResult';


@Injectable()

export class UserService extends BaseService {

  baseUrl = '';

  // Observable navItem source
  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this.authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: HttpClient, configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this.authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
  }

  register(email: string, password: string, firstName: string, lastName: string, location: string): Observable<Object> {
    const body = JSON.stringify({ email, password, firstName, lastName, location });
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });


    return this.http.post(this.baseUrl + '/accounts', body, { headers });



    // .subscribe(
    //   data => {true; },
    //   err => {this.handleError; }
    // )
  }

  login(user: Credentials): Observable<LoginResult> {
    return this.http.post(this.baseUrl + '/auth/login',
      {
        username: user.email,
        password: user.password
      }, { headers: new HttpHeaders({ 'No-Auth': 'True' }) }) as Observable<LoginResult>;
  }



  // login(userName: any, password: any) {
  //   let headers = new HttpHeaders();
  //   headers = headers.append('Content-Type', 'application/json');

  //   return this.http.post(this.baseUrl + '/auth/login', JSON.stringify({ userName, password }), {headers} )
  //     .subscribe(res => {
  //       const authToken = JSON.stringify(res);
  //       localStorage.setItem('auth_token', authToken);
  //       this.loggedIn = true;
  //       this.authNavStatusSource.next(true);
  //       return true;
  //     },
  //       err => (this.handleError))
  //     ;
  // }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    this.authNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }
}
