import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
  })
  export class UserService
      extends BaseService {
      constructor(
          http: HttpClient,
          @Inject('BASE_URL') baseUrl: string) {
          super(http, baseUrl);
      }

     userUrl: string = "https://localhost:8000/";
  
      getData<ApiResult>(
          pageIndex: number,
          pageSize: number,
          sortColumn: string,
          sortOrder: string,
          filterColumn: string,
          filterQuery: string
      ): Observable<ApiResult> {
          var url = this.userUrl + 'api/user';
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
  
      get<User>(id): Observable<User> {
          var url = this.userUrl + "api/user/" + id;
          console.log("URL:" + url)
          return this.http.get<User>(url);
      }
  
      put<User>(item): Observable<User> {
          var url = this.userUrl + "api/user/" + item.id;
          console.log("URL:" + url)
          return this.http.put<User>(url, item);
      }

      post<User>(item: User): Observable<User> {
          var url = this.userUrl + "api/user";
          console.log("URL:" + url);
          console.log("User:" + item);
          return this.http.post<User>(url, item);
      }
  }
