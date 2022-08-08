import { Supplier } from './../../../../_core/_model/setting/supplier';
import { Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { AlertifyService } from "src/app/_core/_service/alertify.service";
import { GridComponent } from "@syncfusion/ej2-angular-grids";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { BaseComponent } from "src/app/_core/_component/base.component";
import { TranslateService } from "@ngx-translate/core";
import { ProcessService } from "src/app/_core/_service/setting/process.service";
import { SupplierService } from "src/app/_core/_service/setting/supplier.service";

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent extends BaseComponent implements OnInit, OnDestroy {
  public toolbarOptions = [ "Add", "Cancel", "Search"];
  public editSettings = {
    showDeleteConfirmDialog: false,
    allowEditing: true,
    allowAdding: true,
    allowDeleting: true,
    mode: "Normal",
  };
  public data: object[];
  filterSettings = { type: "Excel" };
  modelSup: Supplier = {} as Supplier;
  public ProcessData: any = [];
  public fieldsSup: object = { text: "name", value: "id" };
  @ViewChild("grid") grid: GridComponent;
  public textGlueLineName = "Select ";
  public supplier: object[];
  public setFocus: any;
  constructor(
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private service: SupplierService,
    public translate: TranslateService,
    private serviceProcess: ProcessService,
  ) {
    super(translate);
  }
  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnInit(): void {
    this.getAllSupplier();
    this.getAllProcess();
  }
  dataBound() {
  }

  rowDeselected(args) {}

  onChangeSupplier(args) {
    this.modelSup.processID = args.value;
  }

  getAllSupplier() {
    this.service.getAll().subscribe((res) => {
      this.supplier = res;
    });
  }
  getAllProcess() {
    this.serviceProcess.getAll().subscribe((res: any) => {
      this.ProcessData = res;
    });
  }

  actionBegin(args) {

    if (args.requestType === "beginEdit" ) {
      this.modelSup.id = args.rowData.id || 0 ;
      this.modelSup.name = args.rowData.name ;
      this.modelSup.processID = args.rowData.processID ;
    }
    if (args.requestType === "save" ) {
      if (args.action === "edit") {
        this.modelSup.id = args.data.id || 0;
        this.modelSup.name = args.data.name;
        // this.modelSup.ProcessID = args.data.processID;
        this.update(this.modelSup);
      }
      if (args.action === "add") {
        const dataSource = this.grid.dataSource as any
        const exist = dataSource.filter(x => x.name === args.data.name && x.processID === this.modelSup.processID)
        if (this.modelSup.processID === 0) {
          this.alertify.error("Please select Treatment");
          args.cancel = true;
          return ;
        }
        if (exist.length > 0) {
          this.alertify.error("Data already exists");
          args.cancel = true;
          return ;
        }
        this.modelSup.id = 0;
        this.modelSup.name = args.data.name;
        if (args.data.name !== undefined && this.modelSup.processID > 0) {
          this.add(this.modelSup);
        } else {
          this.getAllSupplier();
          this.grid.refresh();
        }
      }
    }
    if (args.requestType === "delete") {
      this.delete(args.data[0].id);
    }
  }

  toolbarClick(args): void {
    switch (args.item.text) {
      case "Excel Export":
        this.grid.excelExport();
        break;
      default:
        break;
    }
  }

  actionComplete(e: any): void {
    if (e.requestType === "add") {
      (e.form.elements.namedItem("name") as HTMLInputElement).focus();
      (e.form.elements.namedItem("id") as HTMLInputElement).disabled = true;
    }
  }

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }
  delete(id) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.deleteTitle,
      this.alert.deleteMessage,
      () => {
        this.service.delete(id).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.deleted_ok_msg);
              this.modelSup.name = "";
              this.modelSup.processID = 0
              this.getAllSupplier();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (err) => this.alertify.warning(this.alert.system_error_msg)
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getAllSupplier();
      }
    );

  }

  update(modelSup) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.service.update(modelSup as Supplier).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.getAllSupplier();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (error) => {
            this.alertify.warning(this.alert.system_error_msg);
          }
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getAllSupplier();
      }
    );
    // this.service.update(modelSup).subscribe((res) => {
    //   this.alertify.success("Updated successfully!");
    //   this.getAllSupplier();
    // });
  }

  add(modelSup) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.createTitle,
      this.alert.createMessage,
      () => {
        this.service.add(modelSup).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.modelSup.name = "";
              this.modelSup.processID = 0
              this.getAllSupplier();
              // this.modalReference.dismiss();

            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }

          },
          (error) => {
            this.alertify.warning(this.alert.system_error_msg);
          }
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getAllSupplier();
      }
    );
    // this.service.add(modelSup).subscribe(() => {
    //   this.alertify.success("Add supplier successfully");
    //   this.getAllSupplier();
    //   this.modelSup.name = "";
    //   this.modelSup.processID = 0
    // });
  }


  NO(index) {
    return ((this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1);
  }

}
