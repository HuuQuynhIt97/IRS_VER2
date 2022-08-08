import { FarmDropdownlistComponent } from './../_component/farm-dropdownlist/farm-dropdownlist.component';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { DropDownListModule, MultiSelectAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CodeTypeDropdownlistComponent } from '../_component/code-type-dropdownlist/code-type-dropdownlist.component';
import { MaskedtimetextboxComponent } from '../_component/maskedtimetextbox/maskedtimetextbox.component';
import { MaskedTextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { TranslateModule } from '@ngx-translate/core';

import { CustomerDropdownlistComponent } from '../_component/customer-dropdownlist/customer-dropdownlist.component';
import { RoomDropdownlistComponent } from '../_component/room-dropdownlist/room-dropdownlist.component';
import { MyCheckboxComponent } from '../_component/my-checkbox/my-checkbox.component';
import { CheckBoxAllModule } from '@syncfusion/ej2-angular-buttons';
import { AccountDropdownlistComponent } from '../_component/account-dropdownlist/account-dropdownlist.component';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';


@NgModule({
  imports: [
    DropDownListModule,
    FormsModule,
    ReactiveFormsModule,
    MaskedTextBoxModule,
    CheckBoxAllModule,
    TranslateModule,
    GridAllModule,
    MultiSelectAllModule
  ],
  declarations: [

    RoomDropdownlistComponent,
    CodeTypeDropdownlistComponent,
    FarmDropdownlistComponent,

    MaskedtimetextboxComponent,

    CustomerDropdownlistComponent,

    MyCheckboxComponent,
    AccountDropdownlistComponent,

  ],
  exports: [

    RoomDropdownlistComponent,
    CodeTypeDropdownlistComponent,
    FarmDropdownlistComponent,
    MaskedtimetextboxComponent,
    CustomerDropdownlistComponent,
    MyCheckboxComponent,
    AccountDropdownlistComponent,


  ]
})
export class Common2Module {
  // 1 cach import khac cua module
  static forRoot(): ModuleWithProviders<Common2Module> {
    return {
      ngModule: Common2Module
    }
  }
}
