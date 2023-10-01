import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DouadyRabbitPageComponent } from "./component/pages/douady-rabbit-page/douady-rabbit-page.component";
import { JuliaSetPageComponent } from "./component/pages/julia-set-page/julia-set-page.component";
import { MainPageComponent } from "./component/pages/main-page/main-page.component";
import { MandelbrotSetPageComponent } from "./component/pages/mandelbrot-set-page/mandelbrot-set-page.component";

const routes: Routes = [
    { path: "main", component: MainPageComponent },
    { path: "mandelbrot-set", component: MandelbrotSetPageComponent },
    { path: "julia-set", component: JuliaSetPageComponent },
    { path: "douady-rabbit", component: DouadyRabbitPageComponent }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes, { enableTracing: false, useHash: true })],
})

export class AppRoutingModule {}