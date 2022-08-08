import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-container3',
  templateUrl: './container3.component.html',
  styleUrls: ['./container3.component.scss']
})
export class Container3Component implements OnInit {
  yearMonth = new Date();
  constructor() { }

  ngOnInit() {
  }

}
