import { Component, Inject, ViewChild } from '@angular/core';
// import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { User } from './user';
import { UserService } from './user.service';
import { ApiResult } from '../base.service';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Project } from '../project/project';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  public displayedColumns: string[] = ['id', 'userName', 'role', 'projectCount'];
  public projects: MatTableDataSource<User>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;

  public defaultSortColumn: string = "id";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "title";
  filterQuery: string = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(    
    private mapsAPILoader: MapsAPILoader,
    private cityService: UserService) {
  }

  ngOnInit() {
    this.loadData(null);
  }

  loadData(query: string = null) {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    if (query) {
      this.filterQuery = query;
    }
    this.getData(pageEvent);
  }

  getData(event: PageEvent) {

    var sortColumn = (this.sort)
      ? this.sort.active
      : this.defaultSortColumn;

    var sortOrder = (this.sort)
      ? this.sort.direction
      : this.defaultSortOrder;

    var filterColumn = (this.filterQuery)
      ? this.defaultFilterColumn
      : null;

    var filterQuery = (this.filterQuery)
      ? this.filterQuery
      : null;

    this.cityService.getData<ApiResult<User>>(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery)
      .subscribe(result => {

        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.projects = new MatTableDataSource<User>(result.data);
        this.mapsAPILoader.load();

      }, error => console.error(error));
  }
}
