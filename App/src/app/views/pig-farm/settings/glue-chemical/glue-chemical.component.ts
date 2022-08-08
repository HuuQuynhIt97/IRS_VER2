import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-glue-chemical',
  templateUrl: './glue-chemical.component.html',
  styleUrls: ['./glue-chemical.component.css']
})
export class GlueChemicalComponent implements OnInit {
  height = innerHeight - 117 - 15;
  constructor() { }

  ngOnInit(): void {
  }

}
