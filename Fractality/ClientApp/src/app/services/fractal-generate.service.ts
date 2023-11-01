import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { AppConfig } from "../app.config";
import { BaseService } from "./base.service";

@Injectable()
export class FractalGenerateService extends BaseService {
    
    constructor(
        http: HttpClient,
        zone: NgZone,
        @Inject('BASE_URL') baseUrl: string,
        protected config: AppConfig
    ) {
        super(http, zone, baseUrl);
    }

    public get baseUrl(): string {
        return "../../../../assets/images/genetare-images";
    }

    public generateMandelbrotSet(): Promise<string> {
        return this.get(`${this.config.fractalGenerateApi}/generate/mandelbrot-set`).then(image => `${this.baseUrl}/${image.body}`);
    }

    public generateJuliaSet(): Promise<string> {
        return this.get(`${this.config.fractalGenerateApi}/generate/julia-set`).then(image => `${this.baseUrl}/${image.body}`);
    }

    public generate(): Promise<string> {
        return this.get(`${this.config.fractalGenerateApi}/generate/test`).then(image => image);
    }
}