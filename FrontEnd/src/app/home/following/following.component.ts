import { Component, OnInit } from '@angular/core';
import { UserInfo } from 'src/app/shared/models/user-info.interface';
import { FollowingService } from 'src/app/shared/services/following.service';

@Component({
  selector: 'app-following',
  templateUrl: './following.component.html',
  styleUrls: ['./following.component.scss']
})
export class FollowingComponent implements OnInit {
  followers: UserInfo[];
  following: UserInfo[];

  constructor(
    private followingService: FollowingService
  ) { }

  ngOnInit() {
    this.followingService.getFollowersList().subscribe(
      (data: Array<UserInfo>)=>{
        this.followers = data;
      }
    );

    this.followingService.getFollowingList().subscribe(
      (data: Array<UserInfo>)=>{
        this.following = data;
      }
    );
  }
}
