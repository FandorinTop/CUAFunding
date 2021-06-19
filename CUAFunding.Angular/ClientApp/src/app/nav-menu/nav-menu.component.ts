import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(public translate: TranslateService){
    console.log("lang: " + translate.get);
  }

  isExpanded = false;

  langChange($event){
    console.log("langChange: "+ $event.value);
    if($event.value == 'en'){
      console.log("changed to eng: "+ $event.value);
      this.setEng();
    }
    else if($event.value == 'ua'){
      console.log("changed to ukarine: "+ $event.value);

      this.setUa();
    } 
    else if($event.value == 'ru'){
      console.log("changed to russia: "+ $event.value);

      this.setRus();
    }
  }

  setEng(){
    this.translate.use('en');
  }
  setRus(){
    this.translate.use('ru');
  }
  setUa(){
    this.translate.use('ua');
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
