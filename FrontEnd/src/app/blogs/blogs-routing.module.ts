import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BlogsComponent } from "./blogs.component";
import { DashboardComponent } from "../home/dashboard/dashboard.component";
import { UserPageComponent } from "./user-page/user-page.component";

const routes: Routes = [
  {
    path: "",
    component: BlogsComponent,
    children: [
      { path: "", redirectTo: "/home/dashboard", pathMatch: "full" },
      {
        path: ":username",
        component: UserPageComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BlogsRoutingModule {}
