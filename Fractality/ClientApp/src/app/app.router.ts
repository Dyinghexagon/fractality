import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { JuliaSetPageComponent } from "./component/pages/julia-set-page/julia-set-page.component";
import { MainPageComponent } from "./component/pages/main-page/main-page.component";
import { MandelbrotSetPageComponent } from "./component/pages/mandelbrot-set-page/mandelbrot-set-page.component";

const routes: Routes = [
    { path: "", component: MainPageComponent, pathMatch: "full" },
    { path: "main", component: MainPageComponent },
    { path: "mandelbrot-set", component: MandelbrotSetPageComponent },
    { path: "julia-set", component: JuliaSetPageComponent }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes, { enableTracing: false, useHash: true })],
})

export class AppRoutingModule {}