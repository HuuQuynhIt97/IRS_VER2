import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-shoe-glue',
  templateUrl: './shoe-glue.component.html',
  styleUrls: ['./shoe-glue.component.css']
})
export class ShoeGlueComponent implements OnInit {
  height = innerHeight - 117 - 15;
  constructor() { }

  ngOnInit(): void {
  }

}
