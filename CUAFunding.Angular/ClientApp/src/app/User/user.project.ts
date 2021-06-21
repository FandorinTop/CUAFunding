import { Component, Inject, ViewChild } from '@angular/core';
// import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { ApiResult } from '../base.service';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { UserProjectService } from './user.project.service';
import { Project } from '../project/project';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'user-project',
  templateUrl: './user.project.html',
  styleUrls: ['./user.project.css']
})
export class ProjectComponent {
  public displayedColumns: string[] = ['id', 'title', 'locationX', 'locationY', 'goal', 'expirationDate'];
  public projects: MatTableDataSource<Project>;

  public mapProject: Project[];

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;

  latitude: number = 50;
  longitude: number = 36;
  zoom: number = 7;

  public defaultSortColumn: string = "title";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "title";
  id: string = "";
  filterQuery: string = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(    
    private cityService: UserProjectService,
    private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
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

    this.cityService.getData<ApiResult<Project>>(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery,
      this.id)
      .subscribe(result => {
        
        this.mapProject = result.data;

        console.log("projects: " + this.mapProject);

        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.projects = new MatTableDataSource<Project>(result.data);

      }, error => console.error(error));
  }
}
