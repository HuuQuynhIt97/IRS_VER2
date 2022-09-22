import { NgModule, APP_INITIALIZER, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { appInitializer } from './_helper/appInitializer';
import { JwtInterceptor } from '@auth0/angular-jwt';
import { AuthService } from './_service/auth.service';
import { SystemLanguageService } from './_service/system-language.service';
import { languagesInitializer } from './_helper/languagesInitializer';
import { sysConfInitializer } from './_helper/sysConfInitializer';
import { SystemConfigService } from './_service/systemconfig.service';
import { ShoeGlueService } from './_service/setting/shoe-glue.service';
import { colorPageSettingConfInitializer } from './_helper/colorPageSettingConfInitializer';


@NgModule({
  declarations: [

  ],
  imports: [CommonModule],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      multi: true,
      deps: [AuthService],
    },
    {
      provide: APP_INITIALIZER,
      useFactory: languagesInitializer,
      multi: true,
      deps: [SystemLanguageService],
    },
    {
      provide: APP_INITIALIZER,
      useFactory: sysConfInitializer,
      multi: true,
      deps: [SystemConfigService],
    },
    {
      provide: APP_INITIALIZER,
      useFactory: colorPageSettingConfInitializer,
      multi: true,
      deps: [ShoeGlueService],
    },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Core Module can only be imported to AppModule.');
    }
  }
}
