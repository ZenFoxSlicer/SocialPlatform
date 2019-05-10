import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {

    apiURI: string;

    constructor() {
        this.apiURI = 'http://localhost:8080/api';
     }

     getApiURI() {
         return this.apiURI;
     }
}
