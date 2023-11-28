import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { TableComponent } from "./table/table.component";
import { MaterialModule } from "../shared/material-module/materiale.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MyPostsComponent } from "./my-posts/my-posts.component";
import { FollowingComponent } from "./following/following.component";
import { UsersGridComponent } from "./following/users-grid/users-grid.component";
import { EditPostComponent } from "./my-posts/edit-post/edit-post.component";
import { UserPageComponent } from '../blogs/user-page/user-page.component';
import { UserComponent } from './user/user.component';

@NgModule({
  declarations: [
    HomeComponent,
    DashboardComponent,
    TableComponent,
    MyPostsComponent,
    FollowingComponent,
    UsersGridComponent,
    EditPostComponent,
    UserComponent,
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [HomeRoutingModule, MaterialModule],
  entryComponents: [EditPostComponent],
})
export class HomeModule {}
