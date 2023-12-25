import { AfterViewInit, Component, ElementRef, OnDestroy, ViewChild } from "@angular/core";
import { FractalEmptyFactory } from "src/app/models/Fractals/fractal-empty-factory.model";
import { JuliaSet } from "src/app/models/Fractals/julia-set.model";
import { MandelbrotSet } from "src/app/models/Fractals/mandelbrot-set.mode;";
import { FractalType } from "src/app/models/Fractals/fractal.model";
import { ClickType, FractalGenerateService, IScreenResolution, ScreenResolutionName } from "src/app/services/fractal-generate.service";
import { TranslateService } from "@ngx-translate/core";
import { Subject, takeUntil } from "rxjs";
import { KeyValue } from "@angular/common";
import { MdbModalRef, MdbModalService } from "mdb-angular-ui-kit/modal";
import { FractalModal } from "../../modals/fractal-modal/fractal-modal.component";

@Component({
    selector: "main-page",
    templateUrl: "./main-page.component.html",
    styleUrls: [ "./main-page.component.scss" ]
})
export class MainPageComponent implements AfterViewInit, OnDestroy {

    @ViewChild("canvas", { static: false }) public canvas!: ElementRef;

    public screenResolutionName = ScreenResolutionName.HD;
    public screenResolution!: IScreenResolution;
    public mandelbrotSet: MandelbrotSet;
    public juliaSet: JuliaSet;
    public fractalTypeEnum = FractalType;
    public fractalType: FractalType;
    public descriptions!: any;
    
    private modalRef: MdbModalRef<FractalModal> | null = null;

    private readonly unsebscribe$ = new Subject<void>();

    constructor(
        private readonly fractalGenerateService: FractalGenerateService,
        private readonly translateService: TranslateService,
        private readonly modalService: MdbModalService
    ) {
        this.mandelbrotSet = FractalEmptyFactory.createEmptyMandelbrotSet();
        this.juliaSet = FractalEmptyFactory.createEmptyJuliaSet();
        this.fractalType = FractalType.mandelbrotSet;
        this.translateService.get([
            "FRACTAL_DESCRIPTION.MANDELBROT_SET",
            "FRACTAL_DESCRIPTION.JULIA_SET"
        ]).pipe(takeUntil(this.unsebscribe$)).subscribe(result => this.descriptions = result);
    }

    public ngAfterViewInit(): void {
       this.selectedFractalType(this.fractalType);
    }

    public ngOnDestroy(): void {
        this.unsebscribe$.next();
        this.unsebscribe$.complete();
    }

    public selectedFractalType(fractalType: FractalType): void {
        this.fractalType = fractalType;
        switch(this.fractalType) {
            case FractalType.mandelbrotSet: {
                this.fractalGenerateService.generateMandelbrotSet(this.screenResolutionName, this.mandelbrotSet, ClickType.None).then((fractal: MandelbrotSet) => {
                    this.prepareCanvas();
                    var context = this.canvas.nativeElement.getContext("2d");
                    if (!context) {
                        return;
                    }
                    fractal.draw(context, this.screenResolution.width, this.screenResolution.height);
                }); 
                break;
            }
            case FractalType.juliaSet: {
                this.fractalGenerateService.generateJuliaSet(this.screenResolutionName, this.juliaSet, ClickType.None).then((fractal: JuliaSet) => {
                    this.prepareCanvas();
                    var context = this.canvas.nativeElement.getContext("2d");
                    if (!context) {
                        return;
                    }
                    fractal.draw(context, this.screenResolution.width, this.screenResolution.height);
                });
                break;
            }
        }
    }

    private prepareCanvas(): void {
        this.screenResolution = this.fractalGenerateService.getScreenResolution(this.screenResolutionName);
        this.canvas.nativeElement.height = 640;
        this.canvas.nativeElement.width = 1160;
    }

    public openModal(): void {
        this.modalRef = this.modalService.open(FractalModal);
    }

}