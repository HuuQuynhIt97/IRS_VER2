import { filter } from 'rxjs/operators';
import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_service/auth.service';

@Directive({
  selector: '[appPermission]'
})
export class PermissionDirective implements OnInit {
  @Input() appFunction: string;
  @Input() appAction: string;

  constructor(private el: ElementRef, private route: ActivatedRoute, private authService: AuthService) {
  }
  ngOnInit() {
    const loggedInUser = this.authService.loggedIn();
    if (loggedInUser) {
      const functionCode = this.route.snapshot.data.functionCode;
      const functions = JSON.parse(localStorage.getItem('functions')) || [];
      const permissions = functions.includes(functionCode);
      if (permissions) {
        this.el.nativeElement.style.display = '';
      } else {
        this.el.nativeElement.style.display = 'none';
      }
    } else {
      this.el.nativeElement.style.display = 'none';
    }
  }
}
