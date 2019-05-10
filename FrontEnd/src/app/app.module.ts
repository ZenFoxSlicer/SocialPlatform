import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserService } from './shared/services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { ConfigService } from './shared/services/config.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule, MatSnackBarModule, MatButtonModule,
  MatIconModule, MatDatepickerModule, MatCardModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MatInputModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    BrowserAnimationsModule,
    MatCardModule,
    ReactiveFormsModule,
    FlexLayoutModule,
  ],
  providers: [
    UserService,
    ConfigService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
