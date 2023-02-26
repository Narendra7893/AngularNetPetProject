import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/User';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {}
  loggedIn = false;
  loggedInUser : any;
  constructor(public accountService : AccountService, private router: Router,
    private toastr: ToastrService) { }

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
      next :()=> this.router.navigateByUrl('/members'),
      error : error => this.toastr.error(error.error)


    })
  }

  logout()
  {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
