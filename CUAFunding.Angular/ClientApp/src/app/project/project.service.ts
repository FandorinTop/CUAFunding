import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
import { Project } from './project';

@Injectable({
    providedIn: 'root',
  })
  export class ProjectService
      extends BaseService {
      constructor(
          http: HttpClient,
          @Inject('BASE_URL') baseUrl: string) {
          super(http, baseUrl);
      }
  
      getData<ApiResult>(
          pageIndex: number,
          pageSize: number,
          sortColumn: string,
          sortOrder: string,
          filterColumn: string,
          filterQuery: string
      ): Observable<ApiResult> {
          var url = this.baseUrl + 'api/Project';
          var params = new HttpParams()
              .set("pageIndex", pageIndex.toString())
              .set("pageSize", pageSize.toString())
              .set("sortColumn", sortColumn)
              .set("sortOrder", sortOrder);
  
          if (filterQuery) {
              params = params
                  .set("filterColumn", filterColumn)
                  .set("filterQuery", filterQuery);
          }
  
          return this.http.get<ApiResult>(url, { params });
      }
  
      get<Project>(id): Observable<Project> {
          var url = this.baseUrl + "api/Project/" + id;
          console.log("URL:" + url)
          return this.http.get<Project>(url);
      }
  
      put<Project>(item): Observable<Project> {
          var url = this.baseUrl + "api/Project/" + item.id;
          console.log("URL:" + url)
          return this.http.put<Project>(url, item);
      }

      post<Project>(item: Project): Observable<Project> {
          var url = this.baseUrl + "api/Project";
          console.log("URL:" + url);
          console.log("Project:" + item);
          return this.http.post<Project>(url, item);
      }
  }
