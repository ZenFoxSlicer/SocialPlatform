import { Component, Input, OnInit } from "@angular/core";
import { UserInfo } from "src/app/shared/models/user-info.interface";
import * as crypto from "crypto-js";
import { FollowingService } from "src/app/shared/services/following.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-users-grid",
  templateUrl: "./users-grid.component.html",
  styleUrls: ["./users-grid.component.scss"],
})
export class UsersGridComponent implements OnInit {
  @Input("users") users: Array<UserInfo> = [];

  constructor(
    private followingService: FollowingService,
    private router: Router,
    ) {}
  ngOnInit() {}

  getInitials(firstName: string, lastName:string) {

    return `${firstName.charAt(0)}${lastName.charAt(0)}`;
  }

  getColor(name: string) {
    const hash = crypto.SHA256(name);
    return `#${hash.toString().substring(0, 6)}`;
  }

  follow(user: UserInfo) {
    this.followingService
      .follow(user.id)
      .subscribe(() => (user.currentUserIsFollowing = true));
  }

  unfollow(user: UserInfo) {
    this.followingService
      .unfollow(user.id)
      .subscribe(() => (user.currentUserIsFollowing = false));
  }

  navigateToUser(userName: string) {
    this.router.navigate(["blogs", userName]).then();
  }
}
