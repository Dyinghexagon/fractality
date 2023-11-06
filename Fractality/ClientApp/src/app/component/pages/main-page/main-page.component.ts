import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import { FractalGenerateService, IScreenResolution, ScreenResolutionName } from "src/app/services/fractal-generate.service";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent implements AfterViewInit {

    @ViewChild("mandelbrotSet", { static: false }) public mandelbrotSetCanvas!: ElementRef;

    public mandelbrotSrc = "../../../../assets/images/genetare-images/mandelbrot-set.png";
    private screenResolutionName = ScreenResolutionName.TwoK;
    public screenResolution!: IScreenResolution;

    constructor(private readonly fractalGenerateService: FractalGenerateService) {}

    public ngAfterViewInit(): void {
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, "draw").then((colors: string[][]) => {
            this.screenResolution = this.fractalGenerateService.getScreenResolution(this.screenResolutionName);
            this.mandelbrotSetCanvas.nativeElement.height = this.screenResolution.height;
            this.mandelbrotSetCanvas.nativeElement.width = this.screenResolution.width;
            this.drawFractal(colors);
        });
    }

    public async scaleFractal(event: MouseEvent): Promise<void> {
        const x = event.pageX - this.mandelbrotSetCanvas.nativeElement.offsetLeft;
        const y = event.pageY - this.mandelbrotSetCanvas.nativeElement.offsetTop;
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, "zoomIn", x, y).then((colors: string[][]) => {
            this.drawFractal(colors);
        });
    }

    private drawFractal(colors: string[][]): void {
        var context = this.mandelbrotSetCanvas.nativeElement.getContext("2d");
        for(let x = 0; x < this.screenResolution.width; x++) {
            for(let y = 0; y < this.screenResolution.height; y++) {
                var roundedX = Math.floor(x);
                var roundedY = Math.floor(y);
                context.fillStyle = `#${colors[x][y]}` || '#000';
                  context.fillRect(roundedX, roundedY, 1, 1);
            }
        }
    }

    public clear(): void {
        this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, "draw").then((colors: string[][]) => {
            this.drawFractal(colors);
        });
    }

}
