import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Marker } from './marker';
import { Project } from '../project/project';

@Component({
  selector: 'map',
  templateUrl: 'map.component.html',
  styleUrls: ['map.component.css'],
})
export class MapComponent {
  title: string = 'CUAFunding project';
  latitude: number;
  longitude: number;
  zoom: number;
  address: string;
  private geoCoder;
  // projects: Project[] = [
  //   {
  //     locationX: 36,
  //     locationY: 49
  //   }
  // ];

  @ViewChild('search')
  public searchElementRef: ElementRef;
  
  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone
  ) { }
  
  mapClicked($event: MouseEvent): void {
    console.log("lat:" + $event.coords.lat);
    console.log("lng:" + $event.coords.lng);
    
    this.longitude = $event.coords.lng;
    this.latitude = $event.coords.lat;

    let project: Project =  <Project>{};
    
    project.locationX = this.longitude;
    project.locationY = this.latitude;
    project.title = "lat:" + $event.coords.lat + "lng:" + $event.coords.lng;

    // this.cleanMap();
    // this.projects.push(project);
  }

  cleanMap(){
    // this.projects = [];
  }

  ngOnInit() {
    this.mapsAPILoader.load().then(() => {
      this.setCurrentLocation();
    });
  }
  
  private setCurrentLocation() {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.zoom = 8;
        this.getAddress(this.latitude, this.longitude);
      });
    }
  }
  
  getAddress(latitude, longitude) {
    this.geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
      if (status === 'OK') {
        if (results[0]) {
          this.zoom = 12;
          this.address = results[0].formatted_address;
        } else {
          window.alert('No results found');
        }
      } else {
        window.alert('Geocoder failed due to: ' + status);
      }
    });
  }
}