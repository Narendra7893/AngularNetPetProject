import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(private accountservice : AccountService, private toastrservice: ToastrService) {
    
  }
  canActivate(): Observable<boolean> {
    return this.accountservice.currentUser$.pipe(
    map(
        user => {if(user) return true;
          else {
            this.toastrservice.error("You shall not pass!");
            return false;
          }
      })
    );
  }
  
}
