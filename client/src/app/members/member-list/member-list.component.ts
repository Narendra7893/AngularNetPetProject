import { Component, OnInit } from '@angular/core';
import { member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members : member[] = [];
  constructor(private memberService : MembersService) { }

  ngOnInit(): void {
    this.loadMembers()
  }

  loadMembers(){
    this.memberService.getMembers().subscribe({
      next : members => this.members = members
    })
  }

}
