import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable, NgZone } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class BaseService {

    private _baseUrl: string;

    constructor(
        protected http: HttpClient,
        protected zone: NgZone,
        @Inject('BASE_URL') baseUrl: string
    ) { 
        this._baseUrl = baseUrl;
    }

    private getHeaders(): HttpHeaders {
        return new HttpHeaders(
            {
                "content-type": "application/json", 
                "cache-control": "no-cache",
                "Accept": "application/json",
            }
        );
    }

    public post(url: string, data: any, silent?: boolean): Promise<any> {
        const observable = this.http.post(url, JSON.stringify(data), { headers: this.getHeaders(), observe: "response", withCredentials: true  });
        return this.subscribe(observable, this._baseUrl + url, silent);
    }

    public get(url: string, silent?: boolean, full: boolean = false): Promise<any> {
        const observable = this.http.get(url, { headers: this.getHeaders(), observe: "response", withCredentials: true });
        return this.subscribe(observable, this._baseUrl + url, silent, full);
    }

    public put(url: string, data: any, silent?: boolean): Promise<any> {
        const observable = this.http.put(url, JSON.stringify(data), { headers: this.getHeaders(), observe: "response", withCredentials: true });
        return this.subscribe(observable, this._baseUrl + url, silent);
    }

    public delete(url: string, silent?: boolean): Promise<any> {
        const observable = this.http.delete(url, { headers: this.getHeaders(), observe: "response" });
        return this.subscribe(observable, this._baseUrl + url, silent);
    }

    protected subscribe(observable: Observable<object>, url: string, silent?: boolean, full: boolean = false): Promise<any> {
        const promise = new Promise((resolve, reject) => {
            observable.subscribe({
                next: (r: any) => {
                    setTimeout(() => {
                        this.zone.run(() => {
                            resolve(r);
                        });
                    });
                },
                error: r => {
                    if (silent) {   // ToDo check
                        if (r.status === 500) {
                            resolve({ code: "500" });
                        } else {
                            resolve(r.error || null);
                        }
                    } else {
                        switch(r.status) {
                            case 400 || 403: {
                                resolve(null);
                                break;
                            }
                            case 401: {
                                reject(r);
                                break;
                            }
                            default: {
                                resolve(r.error || null);
                            }
                        }
                    }
                }
            });
        });

        return promise;
    }

}