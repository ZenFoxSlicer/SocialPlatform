import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ConfigService } from "./config.service";
import { UserInfo } from "../models/user-info.interface";

@Injectable()
export class FollowingService extends BaseService {
  baseUrl = ""
  constructor(
    private http: HttpClient,
    configService: ConfigService
  ) {
    super();
    this.baseUrl = configService.getApiURI();
  }

  follow(id: string): Observable<any> {
    return this.http.get(this.baseUrl + `/followers/follow/${id}`) as Observable<any>;
  }

  unfollow(id: string): Observable<any> {
    return this.http.get(this.baseUrl + `/followers/unfollow/${id}`) as Observable<any>;
  }

  isfollowing(id: string): Observable<any> {
    console.log(id);
    console.log(this.baseUrl + `followers/following/${id}`);
    return this.http.get(this.baseUrl + `/followers/following/${id}`) as Observable<any>;
  }

  getFollowingList(): Observable<Array<UserInfo>> {
    return this.http.get(this.baseUrl + `/followers/following-list`) as Observable<Array<UserInfo>>;
  }

  getFollowersList(): Observable<Array<UserInfo>> {
    return this.http.get(this.baseUrl + `/followers/followers-list`) as Observable<Array<UserInfo>>;
  }
}
