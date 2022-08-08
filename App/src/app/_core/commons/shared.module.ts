import { loadCldr } from '@syncfusion/ej2-base';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { RadioButtonModule, CheckBoxAllModule, SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { DropDownListModule, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { TreeViewAllModule, ToolbarModule } from '@syncfusion/ej2-angular-navigations';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid';
import { DatePickerModule, DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';

declare var require: any;
loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 


loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/vi/ca-gregorian.json'),
  require('cldr-data/main/vi/numbers.json'),
  require('cldr-data/main/vi/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 


loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/zh/ca-gregorian.json'),
  require('cldr-data/main/zh/numbers.json'),
  require('cldr-data/main/zh/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 

const EJ2_MODULE = [
  DropDownListModule,
  DatePickerAllModule,
  TreeGridAllModule,
  ToolbarModule,
  GridAllModule,
  RadioButtonModule,
  TooltipModule,
  CheckBoxAllModule,
  MultiSelectModule,
  TreeViewAllModule,
  DateTimePickerModule,
  DatePickerModule,
  SwitchModule
];

@NgModule({
 imports:      [

  ],
 declarations: [

  ],
 exports:      [
  ...EJ2_MODULE,
  TranslateModule
 ]
})
export class SharedModule {
  // 1 cach import khac cua module
  static forRoot(): ModuleWithProviders<SharedModule> {
    return {
      ngModule: SharedModule
    }
  }
}
