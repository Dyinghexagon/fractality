import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.router';
import { MainPageComponent } from './component/pages/main-page/main-page.component';
import { AppConfig } from './app.config';
import { BaseService } from './services/base.service';
import { UserService } from './services/user.service';
import { FractalGenerateService } from './services/fractal-generate.service';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MdbModalModule } from 'mdb-angular-ui-kit/modal';
import { FractalModal } from './component/modals/fractal-modal/fractal-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    FractalModal
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    RouterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    MdbModalModule
  ],
  providers: [
    AppConfig,
    BaseService,
    UserService,
    FractalGenerateService,
    TranslateService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function createTranslateLoader(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
