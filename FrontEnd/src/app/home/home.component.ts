import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  opened = true;
  constructor(
    private userService: UserService
  ) { }

  ngOnInit() {
  }

  toggleSideNav(){
    this.opened = !this.opened;
  }

  get getUserId(): string{
    return this.userService.getUserName();
  }
}
