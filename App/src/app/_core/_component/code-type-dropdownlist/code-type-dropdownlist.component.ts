
import { DataManager, Query, UrlAdaptor } from '@syncfusion/ej2-data';
import { Component, Input, OnInit, ViewChild, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { environment } from 'src/environments/environment';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-code-type-dropdownlist',
  templateUrl: './code-type-dropdownlist.component.html',
  styleUrls: ['./code-type-dropdownlist.component.scss']
})
export class CodeTypeDropdownlistComponent implements OnInit, OnChanges {
  @Input() id = "codeType-remote";
  @Input() selectedValue: any;
  @Input() placeholder = "";
  @Input() codeType = "";
  @Input() disabled = false;
  @Output() change = new EventEmitter<any>();
  @Output() ngModelChange = new EventEmitter<any>();
  @Output() selectedValueChange = new EventEmitter<any>();
  @ViewChild('codeTypeRemote') public dropdownObj: DropDownListComponent
  @Output('onblur') onblurChange = new EventEmitter<any>();

  public data: DataManager;
  public query: Query ;
  public remoteFields: Object = { text: 'name', value: 'guid' };
  baseUrl = environment.apiUrl;
  take = 10;
  skip = 0;
  public onOpen(args) {
    // let start: number = this.take;
    // let end: number = 5;
    // let listElement: HTMLElement = (this.dropdownObj as any).list;
    // listElement.addEventListener('scroll', () => {
    //   console.log(listElement.scrollTop + listElement.offsetHeight,listElement.scrollHeight )
    //   if ((listElement.scrollTop + listElement.offsetHeight) >= listElement.scrollHeight) {

    //     let filterQuery = this.dropdownObj.query.clone();
    //     this.data.executeQuery(filterQuery.skip(start).take(end)).then((event: any) => {
    //       start = end;
    //       end += 5;
    //       // const unique = [...new Set(event.result.map(item => item.group))];
    //       this.dropdownObj.addItem(event.result as { [key: string]: Object }[]);
    //     }).catch((e: Object) => {
    //     });
    //   }
    // })
  }
  public onFiltering: any = (e: any) => {
    if (e.text === '') {
      e.updateData(this.data);
    } else {
      const query = this.dropdownObj.query.clone().search(e.text, 'name');
      e.updateData(this.data, query);
    }
  };
  public actionComplete(e: any): void {
    e.result = e.result.map(x => {
      let name = x.guid === "" ? this.trans.instant(x.name) : x.name;
      return {
        guid: x.guid,
        name: name
      }
    })
}
  constructor(public trans: TranslateService) {}
  ngOnInit() {
    this.query = new Query()
    .skip(this.skip)
    .take(this.take)
    .addParams("lang", localStorage.getItem('lang'));
    this.data = new DataManager({
      url: `${this.baseUrl}CodeType/GetDataDropdownlist?lang=${localStorage.getItem('lang')}&codeType=${this.codeType}`,
      adaptor: new UrlAdaptor,
      crossDomain: true,
    }, this.query);
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.selectedValue = this.selectedValue || "";
  }
  onChange(args) {
    this.change.emit(args);
  }
  onNgModelChange(value) {
    this.ngModelChange.emit(value);
    this.selectedValueChange.emit(value);
  }
  onblur(e) {
    this.onblurChange.emit(e);
  }
}
