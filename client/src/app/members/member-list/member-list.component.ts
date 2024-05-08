import { MembersService } from 'src/app/_services/members.service';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParams: UserParams;
  user: User;
  genderList = [{value: 'male', display: 'Males'},
               {value: 'female', display: 'Females'}]
  
  constructor(private memberService: MembersService, private accountService: AccountService) {
    this.userParams = this.memberService.getUserParams();
   }

  ngOnInit(): void {
    this.loadeMembers();
  }
  
  loadeMembers(){
    this.memberService.setUserParams(this.userParams);  
    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.results;
      this.pagination = response.pagination;
    })
  }

  resetFilters(){
    this.userParams = this.memberService.resetUserParams();
    this.loadeMembers();
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    this.loadeMembers();
  }
}
