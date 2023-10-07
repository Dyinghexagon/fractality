import { Component } from "@angular/core";
import { Guid } from "guid-typescript";
import { FractalGenerateService } from "src/app/services/fractal-generate.service";
import { UserService } from "src/app/services/user.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent {
    
    constructor(private readonly fractalGenerate: FractalGenerateService) {}

    public async createUser(): Promise<void> {
        this.fractalGenerate.generate();
    }

}