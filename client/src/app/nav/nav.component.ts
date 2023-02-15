import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {}
  loggedIn = false;
  loggedInUser : any;
  constructor(public accountService : AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }
 getCurrentUser(){
this.accountService.currentUser$.subscribe(
{
next: user =>  (this.loggedInUser = user),
error: error => console.log(error),

}
)

 }

  login() {
    this.accountService.login(this.model).subscribe({
      next : response => {
        console.log(response)
      },
      error : error => console.log(error)


    })
  }

  logout()
  {
    this.accountService.logout();
  }

}
