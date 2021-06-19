// import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { BaseFormComponent } from '../base.form.component';
import { Component, OnInit, ViewChild, ElementRef, NgZone } from '@angular/core';

import { ProjectService } from './project.service';
import { Project } from './project';
import { ApiResult } from '../base.service';
import { stringify } from 'querystring';
import { MapsAPILoader, MouseEvent } from '@agm/core';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})
export class ProjectEditComponent
  extends BaseFormComponent {

  latitude: number = 36;
  longitude: number = 50;
  zoom: number = 7;

  // the view title
  title: string;

  projects: Project[] = [];
  // the form model
  form: FormGroup;

  // the city object to edit or create
  project: Project = <Project>{};

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new city,
  // and not NULL when we're editing an existing one.
  id?: string;

  // Activity Log (for debugging purposes)
  activityLog: string = '';

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService) {
    super();
  }

  ngOnInit() {
    this.loadData();
    this.mapsAPILoader.load();
    console.log("project:" + this.project);
    console.log("Id: " + this.activatedRoute.snapshot.paramMap.get('id'));

    this.form = this.fb.group({
      title: ['', Validators.required],
      locationX: ['', [
        Validators.required, Validators.pattern(/^[-]?[0-9]+(\.[0-9]{1,16})?$/)
      ]],
      locationY: ['', [
        Validators.required, Validators.pattern(/^[-]?[0-9]+(\.[0-9]{1,16})?$/)
      ]],
      goal: ['', [
         Validators.pattern(/^[-]?[0-9]+(\.[0-9]{1,6})?$/)
      ]],
      expirationDate: ['', Validators.required]
    })

    // react to form changes
    this.form.valueChanges
      .subscribe(val => {
        console.log("valueChanges long: " + this.longitude)
        console.log("valueChanges latitude: " + this.latitude)
    
        // this.project.locationX = this.longitude;
        // this.project.locationY = this.latitude;

    if(this.projects.length>0){
      this.projects.pop();
    }

    this.projects.push(this.project);

        if (!this.form.dirty) {
          this.log("Form Model has been loaded.");
        }
        else {
          this.log("Form was updated by the user.");
        }
      });

    // react to changes in the form.name control
    this.form.get("name")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("Name has been loaded with initial values.");
        }
        else {
          this.log("Name was updated by the user.");
        }
      });
  }

  mapClicked($event: MouseEvent): void {
    console.log("lat:" + $event.coords.lat);
    console.log("lng:" + $event.coords.lng);
    
    this.longitude = $event.coords.lat;
    this.latitude = $event.coords.lng;
    
    this.project.locationX = this.latitude;
    this.project.locationY = this.longitude;
    this.project.title = "lat:" + $event.coords.lat + "lng:" + $event.coords.lng;

    if(this.projects.length>0){
      this.projects.pop();
    }

    this.projects.push(this.project);
  }

  changeMarkerPosition(){
    this.projects.pop();
    this.projects.push(this.project);
  }

  log(str: string) {
    this.activityLog += "["
      + new Date().toLocaleString()
      + "] " + str + "<br />";
  }

  loadData() {

    // retrieve the ID from the 'id'
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    console.log("Id: " + this.id);
    if (this.id) {
      // EDIT MODE

      // fetch the city from the server
      this.projectService.get<Project>(this.id)
        .subscribe(result => {
        this.project = result;
        this.title = "Edit - " + this.project.title;
        this.zoom=12;
        // update the form with the city value
        this.form.patchValue(this.project);

        if(this.project){
          this.longitude = this.project.locationY;
          this.latitude = this.project.locationX;
          this.projects = [];
          this.projects.push(this.project);
        }
        
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE
      this.title = "Create a new Project";
    }
  }

  onSubmit() {

    var project = (this.id) ? this.project : <Project>{};

    project.title = this.form.get("title").value;
    project.locationY = +this.form.get("locationX").value;
    project.locationX = +this.form.get("locationY").value;
    project.goal = +this.form.get("goal").value;
    project.expirationDate = this.form.get("expirationDate").value;


    if (this.id) {
      console.log("Updating Project" + project);
      // EDIT mode
      this.projectService
        .put<Project>(project)
        .subscribe(result => {

          console.log("Project " + project.id + " has been updated.");

          // go back to cities view
          this.router.navigate(['/admin/projects']);
        }, error => console.error(error));
    }
    else {    
      console.log("Creating Project" + project);

      // ADD NEW mode
      this.projectService
        .post<Project>(project)
        .subscribe(result => {

          console.log("Project " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/admin/projects']);
        }, error => console.error(error));
        this.router.navigate(['/admin/projects']);
    }
  }
}
