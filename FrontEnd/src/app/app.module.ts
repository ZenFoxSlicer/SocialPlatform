import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ConfigService } from './shared/services/config.service';

import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserService } from './shared/services/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FakeBackendInterceptor } from './interceptors/fake-back-end-interceptor.interceptor';
import { AddAuthTokenHeaderInterceptor } from './interceptors/add-token.interceptor';
import { PublicationService } from './shared/services/publication.service';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './shared/material-module/materiale.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginDialogComponent } from './landing-page/login-dialog/login-dialog.component';
import { RegistrationFormComponent } from './landing-page/registration-form/registration-form.component';
import { FollowingService } from './shared/services/following.service';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    MaterialModule,
  ],
  entryComponents: [LoginDialogComponent, RegistrationFormComponent],
  providers: [
    ConfigService,
    UserService,
    PublicationService,
    FollowingService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: FakeBackendInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AddAuthTokenHeaderInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
