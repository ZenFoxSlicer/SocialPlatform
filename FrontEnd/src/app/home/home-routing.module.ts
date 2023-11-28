import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TableComponent } from './table/table.component';
import { FollowingComponent } from './following/following.component';
import { MyPostsComponent } from './my-posts/my-posts.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  {
    path: 'home', component: HomeComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard', component: DashboardComponent,
      },
      {
        path: 'table', component: TableComponent,
      },
      {
        path: 'following', component: FollowingComponent,
      },
      {
        path: 'my-posts', component: MyPostsComponent,
      },
      {
        path: 'user', component: UserComponent,
      },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
