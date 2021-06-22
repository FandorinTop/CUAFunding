import { Component, Inject, ViewChild } from '@angular/core';
// import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';


import { ApiResult } from '../base.service';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Donation } from '../donation/donation';
import { DonationService } from './project.service';

@Component({
  selector: 'app-donationTable',
  templateUrl: './donationTable.component.html',
  styleUrls: ['./donationTable.component.css']
})
export class DonationTableComponent {
  public displayedColumns: string[] = ['projectId', 'projectName', 'message', 'userId', 'value', 'creationDate', 'email'];
  public projects: MatTableDataSource<Donation>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;

  zoom: number = 7;

  public defaultSortColumn: string = "value";
  public defaultSortOrder: string = "asc";

  defaultFilterColumn: string = "value";
  filterQuery: string = null;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(    
    private cityService: DonationService) {
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

    this.cityService.getData<ApiResult<Donation>>(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery)
      .subscribe(result => {
        
        console.log("donations: " + result.data);

        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.projects = new MatTableDataSource<Donation>(result.data);

      }, error => console.error(error));
  }
}
