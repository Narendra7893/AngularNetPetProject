import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { member } from '../_models/Member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseurl = environment.apiurl;
  constructor(private http : HttpClient) { }

  getMembers(){
    return this.http.get<member[]>(this.baseurl + 'users', this.getHttpOptions());
  }

  getMember(username : string){
    return this.http.get<member>(this.baseurl + 'user/' + username, this.getHttpOptions());
  }

  getHttpOptions(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    return {
      headers : new HttpHeaders({
        Authorization : 'Bearer ' + user.token
      })
    }
  }

}
