import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { Guid } from "guid-typescript";
import { AppConfig } from "../app.config";
import { BaseService } from "./base.service";

@Injectable()
export class AuthService extends BaseService {

    constructor(
        http: HttpClient,
        zone: NgZone,
        @Inject('BASE_URL') baseUrl: string,
        protected config: AppConfig
    ) {
        super(http, zone, baseUrl);
    }

    public login(): Promise<Guid> {
        return this.get(`${this.config.authApi}/login`);
    }
    
}