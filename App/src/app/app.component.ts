import { SystemConfigService } from './_core/_service/systemconfig.service';
import { SystemLanguageService } from './_core/_service/system-language.service';
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { environment } from 'src/environments/environment';
import { VersionCheckService } from './_core/_service/version-check.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  titleKey = '';
  sysConf = JSON.parse(localStorage.getItem('sysConf'))
  favIcon: HTMLLinkElement = document.querySelector('#appIcon');
  constructor(
    private versionCheckService: VersionCheckService,
    private title:Title,
    private sysConfService: SystemConfigService,
    private translate:TranslateService
    ) {
  }
  ngOnInit(): void {
    this.titleKey = this.sysConf.SysTitleKey
    this.translate.get(this.titleKey).subscribe(name=>{
      this.title.setTitle(name);
    });
    this.versionCheckService.initVersionCheck(environment.versionCheckURL);
    // this.loadDataSystem();

    this.favIcon.href = this.sysConf.SystemIcon;
  }
  
}


