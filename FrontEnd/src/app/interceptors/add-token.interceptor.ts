import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { getAuthToken } from '../helpers/storage-helper';

@Injectable()
export class AddAuthTokenHeaderInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var token = getAuthToken();
        debugger
        if (token != null) {
            const modifiedRequest = request.clone({
                setHeaders: {
                    'Authorization': `Bearer ${token}`,
                }
            });
            return next.handle(modifiedRequest);
        }
        return next.handle(request);
    }
}