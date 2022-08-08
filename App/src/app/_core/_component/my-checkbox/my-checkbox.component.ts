import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-my-checkbox',
  templateUrl: './my-checkbox.component.html',
  styleUrls: ['./my-checkbox.component.css']
})
export class MyCheckboxComponent implements OnInit, OnChanges {
  @Input() checked: any;
  @Input() label: any = '';
  @Output() checkedChange = new EventEmitter<any>();
  checkedValue = false;
  constructor() { }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.checked != changes.checked.currentValue) {
      this.checked = changes.checked.currentValue;
      this.checkedValue = this.checked === 1 ? true : false;
    }
    if (changes.checked.firstChange) {
      this.checked = changes.checked.currentValue;
      this.checkedValue = this.checked === 1 ? true : false;
    }
  }

  ngOnInit() {
  }
  onCheckedChange(value) {
    this.checked = value === true ? 1 : 0;
    console.log(this.checked);
    this.checkedChange.emit(this.checked)
  }
}
