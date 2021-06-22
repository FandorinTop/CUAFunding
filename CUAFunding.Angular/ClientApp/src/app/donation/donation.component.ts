import { Component, Inject, Input, OnInit } from '@angular/core';
import { Donation } from './donation';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'; 
import { HttpClient } from '@angular/common/http';
import { BaseFormComponent } from '../base.form.component';
import { Observable } from 'rxjs';
import { ProjectService } from '../project/project.service';

@Component({
  selector: 'app-donation',
  templateUrl: './donation.component.html',
  styleUrls: ['./donation.component.css']
})
export class DonationComponent extends BaseFormComponent implements OnInit {
  donation: Donation = <Donation>{};
  form: FormGroup;
  isLoading = false;

  constructor(
     //public modal: NgbActiveModal,
     @Inject(MAT_DIALOG_DATA) public data,
     private activatedRoute: ActivatedRoute,
     public http: HttpClient,
     @Inject('BASE_URL') public baseUrl: string,
     public dialogRef: MatDialogRef<DonationComponent>,
     private route: ActivatedRoute,
     private formBuilder: FormBuilder,
     private router: Router
     ) {    super();
     }
 
  ngOnInit() {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      message: ['', Validators.required],
      value: [ '' , Validators.required],
    }); 
    
  }
 
  onSubmit() { 

    console.log("star donation");
    



    this.donation.userId = this.form.get("email").value;
    this.donation.message = this.form.get("message").value;
    this.donation.value = +this.form.get("value").value;
    this.donation.projectId = this.data;

    this.post(this.donation).subscribe(result => {

      console.log("Donation project id: donate");
      this.dialogRef.close();
    }, error => console.error(error));
    this.isLoading = true;
    

    // this.usersService.updateUser(this.editForm.value).subscribe(x => {
    //   this.isLoading = false;
    //   this.modal.close('Yes');
    // },
    //   error => {
    //     this.isLoading = false;
    //   });
  }

  onClose(){
      this.form.reset();
      this.dialogRef.close();
  }

  post<Donation>(item: Donation): Observable<Donation> {
    var url = this.baseUrl + "api/Donation/AddDonation";
    console.log("URL:" + url);
    console.log("AddDonation:" + item);
    return this.http.post<Donation>(url, item);
}
}