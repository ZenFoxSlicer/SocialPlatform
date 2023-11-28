import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LandingPageRoutingModule } from './landing-page-routing.module';
import { LandingPageComponent } from './landing-page.component';
import { FrontPageComponent } from './front-page/front-page.component';
import { MaterialModule } from '../shared/material-module/materiale.module';
import { LoginDialogComponent } from './login-dialog/login-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

@NgModule({
  declarations: [
    LandingPageComponent,
    FrontPageComponent,
    LoginDialogComponent,
    RegistrationFormComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent],
  imports: [
    CommonModule,
    LandingPageRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    LandingPageRoutingModule,
  ],
  entryComponents: [LoginDialogComponent, RegistrationFormComponent],
})
export class LandingPageModule { }
