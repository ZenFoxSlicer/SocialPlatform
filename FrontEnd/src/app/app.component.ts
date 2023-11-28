import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { LoginDialogComponent } from './landing-page/login-dialog/login-dialog.component';
import { RegistrationFormComponent } from './landing-page/registration-form/registration-form.component';
import { UserService } from './shared/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',

  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'BlogPost';

  constructor(private userService: UserService, public dialog: MatDialog) {}

  get userIsLoggedIn(){
    return this.userService.isLoggedIn();
  }

  dialogConfig: MatDialogConfig = {
    minHeight: "100%",
    minWidth: "60%",
    position: {
      top: "30vh",
    },
  };

  openLoginPopUp() {
    this.dialog.open(LoginDialogComponent, this.dialogConfig);
  }
  openRegisterPopUp() {
    this.dialog.open(RegistrationFormComponent, this.dialogConfig);
  }
  logout() {
    this.userService.logout();
  }
}
