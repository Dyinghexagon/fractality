import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { AppConfig } from "../app.config";
import { BaseService } from "./base.service";

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

    public get baseUrl(): string {
        return "../../../../assets/images/genetare-images";
    }

    public generateMandelbrotSet(screenResolutionName: ScreenResolutionName, clickType: string = "test", mouseX: number = 0, mouseY: number = 0): Promise<string[][]> {
        let screenResolution = this.screenResolutions.get(screenResolutionName);
        return this.get(`${this.config.fractalGenerateApi}/mandelbrot-set?clickType=${clickType}&width=${screenResolution?.width}&height=${screenResolution?.height}&limitIteration=100&mouseX=${mouseX}&mouseY=${mouseY}`).then(image => image.body);
    }

    public generateJuliaSet(): Promise<string> {
        return this.get(`${this.config.fractalGenerateApi}/julia-set`).then(image => `${this.baseUrl}/${image.body}`);
    }

    public generate(): Promise<string> {
        return this.get(`${this.config.fractalGenerateApi}/generate`).then(image => `${this.baseUrl}/${image.body}`);
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