import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
    { path: '', loadChildren: './home/home.module#HomeModule' },
    { path: '', loadChildren: './account/account.module#AccountModule' },
    // { path: '', loadChildren: 'app/account/account.module#AccountModule' },

    { path: '**', redirectTo: 'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
