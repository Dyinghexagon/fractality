import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import { FractalEmptyFactory } from "src/app/models/Fractals/fractal-empty-factory.model";
import { MandelbrotSet } from "src/app/models/Fractals/mandelbrot-set.mode;";
import { ClickType, FractalGenerateService, IScreenResolution, ScreenResolutionName } from "src/app/services/fractal-generate.service";
import { AuthService } from "src/app/services/auth.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent implements AfterViewInit {

    @ViewChild("mandelbrotSet", { static: false }) public mandelbrotSetCanvas!: ElementRef;

    public screenResolutionNameValue = ScreenResolutionName;
    public screenResolutionName = ScreenResolutionName.TwoK;
    public screenResolution!: IScreenResolution;
    public mandelbrotSet: MandelbrotSet;

    constructor(private readonly fractalGenerateService: FractalGenerateService) {
        this.mandelbrotSet = FractalEmptyFactory.CreateEmptyMandelbrotSet();
    }

    public ngAfterViewInit(): void {
        const canvas = this.mandelbrotSetCanvas.nativeElement as HTMLCanvasElement;
        canvas.addEventListener("mousedown", (event: MouseEvent) => {
            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;
            this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, this.mandelbrotSet, ClickType.ZoomIn, x, y).then((fractal: MandelbrotSet) => {
                this.mandelbrotSet = fractal;
                this.drawFractal();
            });
        });
        
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, this.mandelbrotSet, ClickType.None).then((fractal: MandelbrotSet) => {
            this.mandelbrotSet = fractal;
            this.setScreenResolution();
            this.drawFractal();
        }); 
    }

    private drawFractal(): void {
        var context = this.mandelbrotSetCanvas.nativeElement.getContext("2d");
        if (!context) {
            return;
        }
        this.mandelbrotSet.draw(context, this.screenResolution.width, this.screenResolution.height);
    }

    public clear(): void {
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, FractalEmptyFactory.CreateEmptyMandelbrotSet(), ClickType.None).then((fractal: MandelbrotSet) => {
            this.mandelbrotSet = fractal;
            this.drawFractal();
        });
    }

    public selectScreenSolutionType(name: ScreenResolutionName): void {
        this.screenResolutionName = name;
        this.setScreenResolution();
        this.clear();
    }

    private setScreenResolution(): void {
        this.screenResolution = this.fractalGenerateService.getScreenResolution(this.screenResolutionName);
        this.mandelbrotSetCanvas.nativeElement.height = this.screenResolution.height;
        this.mandelbrotSetCanvas.nativeElement.width = this.screenResolution.width;
    }

}
