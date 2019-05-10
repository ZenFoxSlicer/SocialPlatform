import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { homeRouting } from './home.router';
import { AuthGuard } from '../guards/auth.guard';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    RouterModule.forChild(homeRouting),
  ],
  providers: [
    AuthGuard
  ]
})
export class HomeModule { }
