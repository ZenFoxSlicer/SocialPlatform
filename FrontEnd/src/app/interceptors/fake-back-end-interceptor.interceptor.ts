import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { delay, mergeMap, materialize, dematerialize } from 'rxjs/operators';
import { LoginResult } from '../shared/models/LoginResult';
import { Employee } from '../shared/models/employee.model';

@Injectable()
export class FakeBackendInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    switch (true) {
      // case request.url.endsWith('auth/login'):
      //   {
      //     return of(new HttpResponse<LoginResult>({ body: { authToken: 'fake-jwt-token' } }));
      //   }
      case request.url.endsWith('dashboard/pie-chart-data'):
        {
          const response = {
            animationEnabled: true,
            exportEnabled: true,
            title: {
              text: "Basic Column Chart in Angular"
            },
            data: [{
              type: "column",
              dataPoints: [
                { y: 71, label: "Apple" },
                { y: 55, label: "Mango" },
                { y: 50, label: "Orange" },
                { y: 65, label: "Banana" },
                { y: 95, label: "Pineapple" },
                { y: 68, label: "Pears" },
                { y: 28, label: "Grapes" },
                { y: 34, label: "Lychee" },
                { y: 14, label: "Jackfruit" }
              ]
            }]
          };
          return of(new HttpResponse<any>({ body: response }));
        }
    }
    return next.handle(request);
  }
}

