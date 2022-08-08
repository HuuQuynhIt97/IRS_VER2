import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/_core/_service/auth.service';
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  online: number;
  userID: number;
  userName: any;
  modalReference: any;
  data: [] = [];
  firstItem: any;
  constructor(public modalService: NgbModal,
              private authenticationService: AuthService) {
  }
  ngOnInit(): void {
  }
  openModal(ref) {
    this.modalReference = this.modalService.open(ref, { size: 'xl', backdrop: 'static', keyboard: false });
  }
}
