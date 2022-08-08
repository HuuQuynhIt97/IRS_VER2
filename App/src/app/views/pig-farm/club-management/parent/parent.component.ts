import { Subscription } from 'rxjs';
import { FarmService } from 'src/app/_core/_service/farms';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
let innerHeight = window.innerHeight - 92;

@Component({
  selector: 'app-parent',
  templateUrl: './parent.component.html',
  styleUrls: ['./parent.component.scss']
})
export class ParentComponent implements OnInit {

  farm: any;
  active: string;
  subscription: Subscription = new Subscription();
  height = innerHeight - 117 - 15;
  @HostListener('window:resize', ['$event'])
  onResize(event) {
    const h = event.target.innerHeight - 92;
    this.height = h - 117 - 15;

  }
  constructor(private service: FarmService) {

  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  ngOnInit() {
    this.subscription.add(this.service.currentFarm.subscribe(farm=> {
      this.farm =farm;
    }));
    this.active = "Barn";
  }

  onActive(active) {
    this.active = active;
  }

}
