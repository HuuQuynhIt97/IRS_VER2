import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent, QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
import { Tooltip } from '@syncfusion/ej2-angular-popups';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { PigKind } from 'src/app/_core/_model/pig-kind';
import { MessageConstants } from 'src/app/_core/_constants';
import { PigKindService } from 'src/app/_core/_service/pig-kind.service';
@Component({
  selector: 'app-pig-kind',
  templateUrl: './pig-kind.component.html',
  styleUrls: ['./pig-kind.component.css']
})
export class PigKindComponent  extends BaseComponent implements OnInit {
  data: PigKind[] = [];
  password = '';
  modalReference: NgbModalRef;
  
  @ViewChild('grid') public grid: GridComponent;
  model: PigKind;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };

  constructor(
    private service: PigKindService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    public translate: TranslateService,
  ) { super(translate);  }

  ngOnInit() {
    // this.Permission(this.route);
    this.loadData();
  }
  // life cycle ejs-grid

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  onChange(args, data) {
    console.log(args);
    data.isDefault = args.checked;

  }

  actionBegin(args) {
    if (args.requestType === 'save' && args.action === 'add') {
      this.model = {...args.data};

      if (args.data.name === undefined) {
        this.alertify.error('Please key in a name!');
        args.cancel = true;
        return;
      }

      this.create();
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.model = {...args.data};
      this.update();
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }
  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }
  actionComplete(args) {
    // if (args.requestType === 'add') {
    //   args.form.elements.namedItem('name').focus(); // Set focus to the Target element
    // }
  }

  // end life cycle ejs-grid

  // api

  loadData() {
    this.service.getAll().subscribe(data => {
      this.data = data;
    });
  }
  delete(id) {
    this.service.delete(id).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(this.alert.deleted_ok_msg);
          this.loadData();
        } else {
           this.alertify.warning(this.alert.system_error_msg);
        }
      },
      (err) => this.alertify.warning(this.alert.system_error_msg)
    );

  }
  create() {
    this.service.add(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(this.alert.created_ok_msg);
          this.loadData();
          this.model = {} as PigKind;
        } else {
           this.alertify.warning(this.alert.system_error_msg);
        }

      },
      (error) => {
        this.alertify.warning(this.alert.system_error_msg);
      }
    );
  }
  update() {
    this.service.update(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(this.alert.updated_ok_msg);
          this.loadData();
        } else {
          this.alertify.warning(this.alert.system_error_msg);
        }
      },
      (error) => {
        this.alertify.warning(this.alert.system_error_msg);
      }
    );
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }

}
