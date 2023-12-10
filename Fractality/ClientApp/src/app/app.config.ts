import { Injectable } from "@angular/core";

@Injectable()
export class AppConfig {
    public get authApi(): string { return "api/auth" }
    public get fractalGenerateApi(): string { return "api/fractal" }
}