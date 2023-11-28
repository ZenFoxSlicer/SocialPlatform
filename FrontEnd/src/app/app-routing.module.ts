import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./landing-page/landing-page.module').then(m => m.LandingPageModule),
  },
  {
    path: 'blogs',
    loadChildren: () =>
      import('./blogs/blogs.module').then(m => m.BlogsModule),
  },
  {
    path: '',
    loadChildren: () =>
      import('./home/home.module').then(m => m.HomeModule),
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
