import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model : any = {}
@Output()  cancleRegsiter  = new EventEmitter();
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  Register(){
    this.accountService.register(this.model).subscribe({
      next:response =>{
        console.log(response);
        this.Cancle();

      },
      error: error => this.toastr.error(error.error)
      
      

      
     } )
  }
  Cancle(){
    console.log("cancelled");
    this.cancleRegsiter.emit(false);
  }

}
