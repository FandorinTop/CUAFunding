import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AgmCoreModule } from '@agm/core';

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
import { MapComponent } from './map/map.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MainMapComponent } from './mainMap/mainMap.component';
import { FileuploadComponent } from './fileUploader/fileupload.component';
//import { FooterComponent } from './footer/footer.component';

export function HttpLoaderFactory(http: HttpClient){
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BaseFormComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ProjectComponent,
    ProjectEditComponent,
    MainMapComponent,
    MapComponent,
    FileuploadComponent
  ],
  exports:[
    //FooterComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    TranslateModule.forRoot({
      loader:{
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'ru'
    }),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAd2NVFngllAuN-nPmPXKYEkdiApMVKmpk'
    }),
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
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
