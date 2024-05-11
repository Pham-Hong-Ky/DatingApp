import { Photo } from './../_models/photo';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { User } from '../_models/User';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/enviroments/enviroment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentSource.asObservable();

  constructor(private http: HttpClient, private presenceService: PresenceService) { } 

  login(model: any) {
    return this.http.post(this.baseUrl +'Account/login', model).pipe(
      map((response: User) =>{
        const user = response;
        if(user){
          this.setCurrentUser(user);
          this.presenceService.createHubConnection(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'Account/register', model).pipe(
      map((user: User) =>{
        if(user){
          this.setCurrentUser(user);
          this.presenceService.createHubConnection(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User){
    if (user) {
      user.roles = [];
      const roles = this.getDecodedToken(user.token).role;
      Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
      localStorage.setItem('user', JSON.stringify(user));
      this.currentSource.next(user);
  } else {
      console.error('User is null');
  }
  }

  logout(){
    localStorage.removeItem('user');
    this.currentSource.next(null);
    this.presenceService.stopHubConnection();
  }
  getDecodedToken(token){
    return JSON.parse(atob(token.split('.')[1]));
  }
}
