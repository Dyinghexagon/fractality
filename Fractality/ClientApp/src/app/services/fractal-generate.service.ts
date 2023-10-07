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

    public generate(): Promise<void> {
        return this.get(`${this.config.fractalGenerateApi}/generate`);
    }
}