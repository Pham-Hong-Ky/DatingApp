import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'http://localhost:5086/api/';
  private currentSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentSource.asObservable();

  constructor(private http: HttpClient) { } 

  login(model: any) {
    return this.http.post(this.baseUrl +'Account/login', model).pipe(
      map((response: User) =>{
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentSource.next(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'Account/register', model).pipe(
      map((user: User) =>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentSource.next(user);
        }
        return user;
      })
    )
  }
  setCurrentUser(user: User){
    this.currentSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentSource.next(null);
  }
}
