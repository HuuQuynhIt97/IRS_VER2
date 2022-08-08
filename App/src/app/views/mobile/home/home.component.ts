import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public router: Router) { }

  ngOnInit() {
    let user = JSON.parse(localStorage.getItem('user'));
    if (!user) {
      this.router.navigate(['/mobile/login']);
    }
  }
  backDesktop() {
    this.router.navigate(["/login"]);
  }
  goToDetail() {
    this.router.navigate(['/mobile/detail']);
  }
  goToMakeOrder(type) {
    this.router.navigate([`/mobile/pigdata/${type}`]);
  }
  noFunction() {
    alert('This function is not ready!')
  }

}
