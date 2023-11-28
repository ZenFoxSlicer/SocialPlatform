import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Employee } from '../models/employee.model';


@Injectable({
  providedIn: 'root'
})
export class TableService {

  baseUrl = '';

  constructor(
    configService: ConfigService,
    private http: HttpClient
  ) {
    this.baseUrl = configService.getApiURI();
  }
  getData(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/people`);
  }

  getValuesData(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/values`);
  }

  addEmployee(employee: Employee): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/people`, employee);
  }

  deleteEmployee(employeeName: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/people/${employeeName}`);
  }
}
