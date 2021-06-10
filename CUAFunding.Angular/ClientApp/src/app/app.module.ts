import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProjectComponent } from './project/project.component';
import { AngularMaterialModule } from './angular-material.module';
import { ProjectEditComponent } from './project/project-edit.component';
import { BaseFormComponent } from './base.form.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BaseFormComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ProjectComponent,
    ProjectEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'admin/projects', component: ProjectComponent },
      { path: 'admin/project/:id', component: ProjectEditComponent },
      { path: 'admin/project', component: ProjectEditComponent }
    ]),
    ReactiveFormsModule,
    BrowserAnimationsModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
