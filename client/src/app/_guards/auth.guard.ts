import { CanActivate } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate  {
  constructor(private accountService: AccountService, private toastr: ToastrService) {}
  canActivate(): Observable<boolean>{
    return this.accountService.currentUser$.pipe(
      map( user => {
        if(user) return true;
        this.toastr.error('you shall not pass!')
      })
    );
  }   
}
