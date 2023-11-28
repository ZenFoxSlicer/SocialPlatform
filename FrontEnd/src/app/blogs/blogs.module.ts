import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogsRoutingModule } from './blogs-routing.module';
import { BlogsComponent } from './blogs.component';
import { UserPageComponent } from './user-page/user-page.component';
import { MaterialModule } from '../shared/material-module/materiale.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    BlogsComponent,
    UserPageComponent,
  ],
  imports: [
    CommonModule,
    BlogsRoutingModule,
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class BlogsModule { }
