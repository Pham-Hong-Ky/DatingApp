import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users : any;

  constructor (private http: HttpClient) { }

  ngOnInit() {
    this.getUsers();
  }
  
  getUsers() {
    this.http.get('http://localhost:5086/api/User').subscribe({
        next: response => this.users = response,
        error: error => console.log(error)
      })
  }
}

