import { Component, OnInit } from "@angular/core";
import { LoginDialogComponent } from "../login-dialog/login-dialog.component";
import { RegistrationFormComponent } from "../registration-form/registration-form.component";
import { MatDialog } from "@angular/material/dialog";
import { UserService } from "src/app/shared/services/user.service";

@Component({
  selector: "app-front-page",
  templateUrl: "./front-page.component.html",
  styleUrls: ["./front-page.component.scss"],
})
export class FrontPageComponent implements OnInit {
  ngOnInit() {}

}
