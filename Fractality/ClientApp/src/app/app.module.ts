import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.router';
import { MainPageComponent } from './component/pages/main-page/main-page.component';
import { MandelbrotSetPageComponent } from './component/pages/mandelbrot-set-page/mandelbrot-set-page.component';
import { JuliaSetPageComponent } from './component/pages/julia-set-page/julia-set-page.component';
import { DouadyRabbitPageComponent } from './component/pages/douady-rabbit-page/douady-rabbit-page.component';
import { HeaderComponent } from './component/layouts/header/header.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    MandelbrotSetPageComponent,
    JuliaSetPageComponent,
    DouadyRabbitPageComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
