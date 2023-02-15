import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMode = false;
  constructor() { }
registerModeToggle(){
  this.registerMode = !this.registerMode;
}
  ngOnInit(): void {
  }

  cancleRegisterMode(event: boolean){
    this.registerMode = event;
  }

}
