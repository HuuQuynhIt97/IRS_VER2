import { AfterViewChecked, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-maskedtimetextbox',
  templateUrl: './maskedtimetextbox.component.html',
  styleUrls: ['./maskedtimetextbox.component.css']
})
export class MaskedtimetextboxComponent implements OnInit, AfterViewChecked {
  @Input() disabled = true;
  @Input() selectedValue = "";
  id = Math.random();
  @Output() selectedValueChange = new EventEmitter<any>();
  @Output('onblur') onblurChange = new EventEmitter<any>();
  constructor(private cdRef:ChangeDetectorRef) { }
  ngAfterViewChecked()
  {
    this.cdRef.detectChanges();
  }
  ngOnInit(): void {
  }
  onChange(args) {
    this.selectedValue = args.value || "";
    if (this.selectedValue?.length === 4) {
      let array =  this.selectedValue.split('');
      this.selectedValue = `${array[0]}${array[1]}:${array[2]}${array[3]}`;
    }
    this.selectedValueChange.emit(this.selectedValue);
  }
  onblur(e) {
    this.onblurChange.emit(e);
  }
}
