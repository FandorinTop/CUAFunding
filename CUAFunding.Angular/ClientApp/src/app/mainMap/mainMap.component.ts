import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Project } from '../project/project';
import { ApiResult } from '../base.service';
import { ProjectService } from '../project/project.service';

@Component({
  selector: 'mainMap',
  templateUrl: 'mainMap.component.html',
  styleUrls: ['mainMap.component.css'],
})
export class MainMapComponent {
  title: string = 'CUAFunding project';
  latitude: number;
  longitude: number;
  zoom: number;
  address: string;
  private geoCoder;
  projects: Project[] = [];
  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "title";
  public defaultSortOrder: string = "asc";

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,
    private cityService: ProjectService) {
  }


  defaultFilterColumn: string = "title";
  filterQuery: string = null;

  ngOnInit() {
    this.loadData(null);
    this.mapsAPILoader.load().then(() => {
      this.setCurrentLocation();
    });
  }

  loadData(query: string = null) {
      this.getData();
  }

  getData() {
    this.cityService.getData<ApiResult<Project>>(
      0,
      1000,
      "id",
      "asc",
      "id",
      "")
      .subscribe(result => {
        
        this.projects = result.data;
      }, error => console.error(error));
  }
  @ViewChild('search')
  public searchElementRef: ElementRef;

  
  private setCurrentLocation() {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.zoom = 8;
      });
    }
  }
}