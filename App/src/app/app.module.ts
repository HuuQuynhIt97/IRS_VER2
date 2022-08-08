import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { ForgotUsernameComponent } from './views/forgot-username/forgot-username.component';
import { ResetPasswordComponent } from './views/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './views/forgot-password/forgot-password.component';
import { HomeComponent } from './views/home/home.component';
import { BrowserModule, Title } from '@angular/platform-browser';
import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './views/layout/layout/layout.component';
import { HeaderComponent } from './views/layout/header/header.component';
import { FooterComponent } from './views/layout/footer/footer.component';
import { BreadcrumbComponent } from './views/layout/breadcrumb/breadcrumb.component';
import { LoginComponent } from './views/login/login.component';

import { P404Component } from './views/p404/p404.component';
import { P500Component } from './views/p500/p500.component';

// service
import { AlertifyService } from './_core/_service/alertify.service';
import { AuthService } from './_core/_service/auth.service';
import { AuthGuard } from './_core/_guards/auth.guard';
import { NgxSpinnerModule } from 'ngx-spinner';
// handle err
import { ErrorInterceptorProvider } from './_core/_helper/error.interceptor';

export function tokenGetter() {
  const token = localStorage.getItem('token');
  let pattern = /^[A-Za-z0-9-_=]+\.[A-Za-z0-9-_=]+\.?[A-Za-z0-9-_.+/=]*$/;
  let result = pattern.test(token);
  if (result) {
    return token;
  }
  localStorage.removeItem('user');
  localStorage.removeItem('token');
  localStorage.removeItem('refresh-token');
  localStorage.removeItem('login-event');
  localStorage.removeItem('functions');
  localStorage.removeItem('menuItem');
  localStorage.removeItem('farmGuid');
  localStorage.removeItem('menus');
  return '';
}
// resolvers


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SafePipeModule } from 'safe-pipe';


// module
import { MomentModule } from 'ngx-moment';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { BasicAuthInterceptor } from './_core/_helper/basic-auth.interceptor';

import { CoreModule } from './_core/core.module';
import { VersionCheckService } from './_core/_service/version-check.service';
import { ChangePasswordComponent } from './views/pig-farm/change-password/change-password.component';
import { ProfileComponent } from './views/pig-farm/profile/profile.component';
import { NgbModule, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { CustomLoader } from './_core/_helper/custom-loader';

import { AccumulationChartAllModule, CategoryService, ChartAllModule, ChartModule, ColumnSeriesService, LegendService } from '@syncfusion/ej2-angular-charts';
import { Common2Module } from './_core/commons/common2.module';
import { DashboardService } from './_core/_service/dashboard.service';
import { ClubManagementModule } from './views/pig-farm/club-management/club-management.module';
let lang = localStorage.getItem('lang');
if (!lang) {
  localStorage.setItem('lang', 'tw');
  lang = localStorage.getItem('lang');
}
@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    HeaderComponent,
    FooterComponent,
    BreadcrumbComponent,
    LoginComponent,
    P404Component,
    P500Component,
    HomeComponent,
    ChangePasswordComponent,
    ProfileComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    ForgotUsernameComponent,

  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    HttpClientModule,
    SafePipeModule,
    MomentModule,
    InfiniteScrollModule,
    CoreModule,
    NgxSpinnerModule,
    NgbModule,
    ChartAllModule,
    GridAllModule,
    AccumulationChartAllModule,
    Common2Module,
    JwtModule.forRoot({
      config: {
        tokenGetter
      }
    }),
    TranslateModule.forRoot({
      loader: {provide: TranslateLoader, useClass: CustomLoader},
      defaultLanguage: lang
    }),
    SharedModule.forRoot(),
    ClubManagementModule,
  ],
  providers: [
    Title,
    CategoryService, LegendService, ColumnSeriesService,
    NgbTooltipConfig,
    AuthGuard,
    AlertifyService,
    VersionCheckService,
    ErrorInterceptorProvider,
    AuthService,
    DatePipe,
    DashboardService,
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

