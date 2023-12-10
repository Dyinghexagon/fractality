import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import { FractalEmptyFactory } from "src/app/models/Fractals/fractal-empty-factory.model";
import { JuliaSet } from "src/app/models/Fractals/julia-set.model";
import { MandelbrotSet } from "src/app/models/Fractals/mandelbrot-set.mode;";
import { ClickType, FractalGenerateService, IScreenResolution, ScreenResolutionName } from "src/app/services/fractal-generate.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent implements AfterViewInit {

    @ViewChild("mandelbrotSet", { static: false }) public mandelbrotSetCanvas!: ElementRef;
    @ViewChild("juliaSet", { static: false }) public juliaSetCanvas!: ElementRef;

    public screenResolutionNameValue = ScreenResolutionName;
    public screenResolutionName = ScreenResolutionName.TwoK;
    public screenResolution!: IScreenResolution;
    public mandelbrotSet: MandelbrotSet;
    public juliaSet: JuliaSet;

    constructor(private readonly fractalGenerateService: FractalGenerateService) {
        this.mandelbrotSet = FractalEmptyFactory.createEmptyMandelbrotSet();
        this.juliaSet = FractalEmptyFactory.createEmptyJuliaSet();
    }

    public ngAfterViewInit(): void {
        const canvas = this.mandelbrotSetCanvas.nativeElement as HTMLCanvasElement;
        canvas.addEventListener("mousedown", (event: MouseEvent) => {
            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;
            this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, this.mandelbrotSet, ClickType.ZoomIn, x, y).then((fractal: MandelbrotSet) => {
                this.mandelbrotSet = fractal;
                this.drawMandelbrotSet();
            });
        });
        
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, this.mandelbrotSet, ClickType.None).then((fractal: MandelbrotSet) => {
            this.mandelbrotSet = fractal;
            this.setScreenResolutionFromMandelbrotSet();
            this.drawMandelbrotSet();
        }); 

        const rel = 0.74543;
        const im = 0.11301;
        const limitIteration = 1000;

        this.fractalGenerateService.generateJuliaSet(this.screenResolutionName, this.juliaSet, ClickType.None, limitIteration, rel, im).then((fractal : JuliaSet) => {
            this.juliaSet = fractal;
            this.setScreenResolutionFromJuliatSet();
            this.drawJuliaSet();
        })
    }

    private drawMandelbrotSet(): void {
        var context = this.mandelbrotSetCanvas.nativeElement.getContext("2d");
        if (!context) {
            return;
        }
        this.mandelbrotSet.draw(context, this.screenResolution.width, this.screenResolution.height);
    }

    private drawJuliaSet(): void {
        var context = this.juliaSetCanvas.nativeElement.getContext("2d");
        if (!context) {
            return;
        }
        this.juliaSet.draw(context, this.screenResolution.width, this.screenResolution.height);
    }

    public clear(): void {
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, FractalEmptyFactory.createEmptyMandelbrotSet(), ClickType.None).then((fractal: MandelbrotSet) => {
            this.mandelbrotSet = fractal;
            this.drawMandelbrotSet();
        });
    }

    public selectScreenSolutionType(name: ScreenResolutionName): void {
        this.screenResolutionName = name;
        this.setScreenResolutionFromMandelbrotSet();
        this.clear();
    }

    private setScreenResolutionFromMandelbrotSet(): void {
        this.screenResolution = this.fractalGenerateService.getScreenResolution(this.screenResolutionName);
        this.mandelbrotSetCanvas.nativeElement.height = this.screenResolution.height;
        this.mandelbrotSetCanvas.nativeElement.width = this.screenResolution.width;
    }

    private setScreenResolutionFromJuliatSet(): void {
        this.screenResolution = this.fractalGenerateService.getScreenResolution(this.screenResolutionName);
        this.juliaSetCanvas.nativeElement.height = this.screenResolution.height;
        this.juliaSetCanvas.nativeElement.width = this.screenResolution.width;
    }


}
