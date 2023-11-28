import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  baseUrl = '';

  constructor(
    configService: ConfigService,
    private http: HttpClient
  ) {
    this.baseUrl = configService.getApiURI();
  }
  getData(): Observable<any> {
    return this.http.get<any>(this.baseUrl + '/dashboard/pie-chart-data');
  }
}
