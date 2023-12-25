import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MainPageComponent } from "./component/pages/main-page/main-page.component";

const routes: Routes = [
    { path: "", component: MainPageComponent, pathMatch: "full" },
    { path: "**", component: MainPageComponent },
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes, { enableTracing: false, useHash: true })],
})

export class AppRoutingModule {}