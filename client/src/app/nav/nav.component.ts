import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/internal/Observable';
import { User } from '../_models/User';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = { };

  constructor(public accountService: AccountService, 
    private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
   }

  login() { 
    this.accountService.login(this.model).subscribe(
      {
        next: response =>this.router.navigateByUrl('/members')
      })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('*/')
  }
  
}
