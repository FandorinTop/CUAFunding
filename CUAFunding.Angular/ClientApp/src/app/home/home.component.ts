import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Project } from '../project/project';
import { ApiResult } from '../base.service';
import { ProjectService } from '../project/project.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  defaultFilterColumn: string = "title";
  filterQuery: string = null;
  title: string = 'CUAFunding project';
  latitude: number = 10;
  longitude: number = 10;
  zoom: number = 10;
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
  ngOnInit() {
    this.mapsAPILoader.load();
    this.loadData();
    }

  loadData(query: string = null) {
      this.getData();
  }

  getData() {
    this.cityService.getData<ApiResult<Project>>(
      0,
      1000,
      "",
      "",
      "",
      "")
      .subscribe(result => {    
        console.log(result); 
        //this.projects = result.data;

        result.data.forEach(element => {
          this.projects.push(element);
        });

        console.log("afterForeach: " + this.projects);

        console.log("projects:");
        this.projects.forEach(element => {
          console.log(element);
        });
      }, error => console.error(error));
  }
  @ViewChild('search')
  public searchElementRef: ElementRef;
}

