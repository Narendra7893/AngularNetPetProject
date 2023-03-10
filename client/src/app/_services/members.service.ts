import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  members : Member[] = [];
  baseurl = environment.apiurl;
  constructor(private http : HttpClient) { }

  getMembers(){
    if(this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseurl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username : string){
    const member = this.members.find(x => x.userName == username);
    if(member) return of(member);
    return this.http.get<Member>(this.baseurl + 'users/' + username);
  }

  updateMember(member: Member){
    return this.http.put(this.baseurl + 'users' ,member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      } )
    );
  }

  SetMainPhoto(photoId : number)
  {
    return this.http.put(this.baseurl + 'users/set-main-photo/' + photoId,{});
  }

  DeletePhoto(photoId : number)
  {
    return this.http.delete(this.baseurl+'users/delete-photo/'+photoId);
  }

  // getHttpOptions(){
  //   const userString = localStorage.getItem('user');
  //   if(!userString) return;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers : new HttpHeaders({
  //       Authorization : 'Bearer ' + user.token
  //     })
  //   }
  // }

}
