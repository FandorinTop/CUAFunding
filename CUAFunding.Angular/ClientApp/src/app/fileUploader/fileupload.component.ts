import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http'
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fileupload',
  templateUrl: './fileupload.component.html',
  styles: []
})
export class FileuploadComponent implements OnInit {
  isUpload: boolean;
  baseUrl: string;
  progress: number;
  Id: string;
  public profileForm: FormGroup;
  @ViewChild('labelImport', { static: true })
  labelImport: ElementRef;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,     private activatedRoute: ActivatedRoute ) {
    this.baseUrl = baseUrl;
  }
  onSubmit() {
    console.log(this.profileForm.value, this.profileForm.valid);
    const formData = new FormData();
    for (const key of Object.keys(this.profileForm.value)) {
      const value = this.profileForm.value[key];
      formData.append(key, value);
    }
    this.http.post(this.baseUrl + 'api/project/ChangeProjectImage', formData, {
      reportProgress: true,
      observe: 'events'
    }).subscribe(event => {
      this.isUpload = false;

      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round((100 * event.loaded) / event.total);
      }
      if (event.type === HttpEventType.Response) {
        console.log(event.body);
        this.isUpload = true;
        this.progress = 0;
        // this.profileForm.reset();
      }
    });
  }
  ngOnInit(): void {
    this.profileForm = new FormGroup({
      id: new FormControl('', [Validators.required]),
      picture: new FormControl('', [Validators.required])
    });

    this.Id = this.activatedRoute.snapshot.paramMap.get('id');
    console.log("formId: " + this.activatedRoute.snapshot.paramMap.get('id'));

    //this.profileForm.patchValue({id: this.activatedRoute.snapshot.paramMap.get('id')})
  }
  get form() {
    return this.profileForm.controls;
  }

  upload(files) {
    if (files.length === 0)
      return;

    const formData = new FormData();

    for (const file of files) {
      formData.append(file.name, file);
    }

    const uploadReq = new HttpRequest('POST', this.baseUrl + 'FileManagement/upload', formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
      };
    });
  }

onFileChanged(event) {
  if (event.target.files.length > 0) {
    const file = event.target.files[0];
    this.labelImport.nativeElement.innerText = file.name;
    this.profileForm.patchValue({
      picture: file,
    });
  }
}
}