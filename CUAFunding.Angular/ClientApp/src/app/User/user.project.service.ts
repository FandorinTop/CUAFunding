import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
  })
  export class UserProjectService {

          constructor(
          public http: HttpClient,
          @Inject('BASE_URL') public baseUrl: string) {
          
      }
  
      getData<ApiResult>(
          pageIndex: number,
          pageSize: number,
          sortColumn: string,
          sortOrder: string,
          filterColumn: string,
          filterQuery: string,
          userId: string,
      ): Observable<ApiResult> {
          var url = this.baseUrl + 'UserProject';
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
  }
