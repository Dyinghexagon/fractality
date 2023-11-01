import { Component, OnInit } from "@angular/core";
import { FractalGenerateService } from "src/app/services/fractal-generate.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent implements OnInit {
    
    public imageUrls: Map<FractalTypes, string> = new Map<FractalTypes, string>();
    public fractalTypes = FractalTypes;
    constructor(private readonly fractalGenerateService: FractalGenerateService) {}

    public async ngOnInit(): Promise<void> {
        this.imageUrls.set(FractalTypes.mandelbrotSet, await this.fractalGenerateService.generateMandelbrotSet());
        this.imageUrls.set(FractalTypes.juliaSet, await this.fractalGenerateService.generateJuliaSet());
    }

    public async generate(): Promise<void> {
        const test = await this.fractalGenerateService.generate();
        console.warn(test);
    }

}

export enum FractalTypes {
    mandelbrotSet = "mandelbrot-set",
    juliaSet = "julia-set",
}