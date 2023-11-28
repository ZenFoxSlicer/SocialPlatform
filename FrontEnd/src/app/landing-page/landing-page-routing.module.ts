import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LandingPageComponent } from './landing-page.component';
import { FrontPageComponent } from './front-page/front-page.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

const routes: Routes = [
  {
    path: '', component: LandingPageComponent,
    children: [
      { path: '', redirectTo: 'landing-page', pathMatch: 'full' },
      {
        path: 'landing-page', component: FrontPageComponent,
      },
      {
        path: 'forgot-password', component:ForgotPasswordComponent
      },
      {
        path: 'reset-password', component:ResetPasswordComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LandingPageRoutingModule { }
