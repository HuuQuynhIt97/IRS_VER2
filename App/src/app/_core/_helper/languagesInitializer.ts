import { SystemLanguageService } from "../_service/system-language.service";
import { SystemConfigService } from "../_service/systemconfig.service";

export function languagesInitializer(
  service: SystemLanguageService,
  sysConfService: SystemConfigService,

  ) {
    return () =>
      new Promise((resolve, reject) => {
            //  console.log('reload app');
              service.getLanguages(localStorage.getItem('lang') || 'tw').subscribe(data => {
                localStorage.setItem('languages', JSON.stringify(data));
            //  console.log('luu lang vao local store');

              }).add(resolve);

              // sysConfService.loadDataSystem().subscribe(data => {
              //   localStorage.setItem('sysConf', JSON.stringify(data));
              //   console.log('luu sysConf vao local store');
              // });
        });
}
