import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { RouterModule } from '@angular/router';
import { accountRouting } from './account.router';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatSnackBarModule, MatButtonModule, MatIconModule, MatDatepickerModule, MatCardModule } from '@angular/material';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { UserService } from '../shared/services/user.service';
import { ConfigService } from '../shared/services/config.service';

@NgModule({
  declarations: [
    RegistrationFormComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatCardModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    RouterModule.forChild(accountRouting),
  ],
  providers: [
    UserService,
    ConfigService
  ]
})
export class AccountModule { }
