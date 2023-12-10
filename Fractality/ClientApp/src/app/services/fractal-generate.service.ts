import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { AppConfig } from "../app.config";
import { BaseService } from "./base.service";
import { FractalModel, IFractalModel } from "../models/Fractals/fractal.model";
import { MandelbrotSet } from "../models/Fractals/mandelbrot-set.mode;";
import { JuliaSet } from "../models/Fractals/julia-set.model";

@Injectable()
export class FractalGenerateService extends BaseService {

    private screenResolutions: Map<ScreenResolutionName, IScreenResolution>;
    
    constructor(
        http: HttpClient,
        zone: NgZone,
        @Inject('BASE_URL') baseUrl: string,
        protected config: AppConfig
    ) {
        super(http, zone, baseUrl);
        this.screenResolutions = new Map<ScreenResolutionName, IScreenResolution>();
        this.screenResolutions.set(ScreenResolutionName.HD, { height: 720, width: 1280 });
        this.screenResolutions.set(ScreenResolutionName.FullHD, { height: 1080, width: 1920 });
        this.screenResolutions.set(ScreenResolutionName.TwoK, { height: 1440, width: 2560 });
        this.screenResolutions.set(ScreenResolutionName.FourK, { height: 2160, width: 3840 });
    }

    public getScreenResolution(screenResolutionName: ScreenResolutionName): IScreenResolution {
        const screenResolution = this.screenResolutions.get(screenResolutionName);

        if (!screenResolution) {
            return { height: 1080, width: 1920 };
        }

        return screenResolution;
    }

    public generateMandelbrotSet(
        screenResolutionName: ScreenResolutionName,
        fractal: FractalModel,
        clickType: ClickType = ClickType.None, 
        mouseX: number = 0, 
        mouseY: number = 0
        ): Promise<MandelbrotSet> {
        let screenResolution = this.screenResolutions.get(screenResolutionName);
        const urlRoot = `${this.config.fractalGenerateApi}/mandelbrot-set?`;
        const query = [
            `clickType=${clickType}`,
            `width=${screenResolution?.width}`,
            `height=${screenResolution?.height}`,
            `limitIteration=100`,
            `mouseX=${mouseX}`,
            `mouseY=${mouseY}`
        ];
        return this.post(urlRoot + query.join("&"), fractal).then(result => new MandelbrotSet(result.body));
    }

    public generateJuliaSet(
        screenResolutionName: ScreenResolutionName,
        fractal: FractalModel,
        clickType: ClickType = ClickType.None,
        limitIteration: number = 100,
        rel: number = 0.74543,
        im: number = 0.11301
    ): Promise<JuliaSet> {
        let screenResolution = this.screenResolutions.get(screenResolutionName);
        const urlRoot = `${this.config.fractalGenerateApi}/julia-set?`;
        const query = [
            `clickType=${clickType}`,
            `width=${screenResolution?.width}`,
            `height=${screenResolution?.height}`,
            `limitIteration=${limitIteration}`,
            `rel=${rel}`,
            `im=${im}`
        ];
        console.warn(rel);
        console.warn(im);
        return this.post(urlRoot + query.join("&"), fractal).then(result => new JuliaSet(result.body));
    }
}

export enum ScreenResolutionName {
    HD,
    FullHD,
    TwoK,
    FourK
}

export interface IScreenResolution {
    height: number;
    width: number;
}

export enum ClickType
{
    ZoomIn,
    Middle,
    ZoomOut,
    None
}