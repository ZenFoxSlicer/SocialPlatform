import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { ConfigService } from "./config.service";
import { Publication } from "../models/publication.interface";
import { BaseService } from "./base.service";
import { PaginatedRequest } from "../models/paginated-request.model";
import { PaginatedResponse } from "../models/paginated-response.model";
import { PublicationExternal } from "../models/publication-external.inteface";
import { PostedComment } from "../models/comment.interface";

@Injectable()
export class PublicationService extends BaseService {
  baseUrl = "";

  constructor(private http: HttpClient, configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
  }

  upsertPublication(form: Publication): Observable<any> {
    return this.http.post(
      this.baseUrl + "/publications/upsert",
      form
    ) as Observable<any>;
  }

  getData(
    paginatedRequest: PaginatedRequest
  ): Observable<PaginatedResponse<Publication>> {
    return this.http.post<PaginatedResponse<Publication>>(
      this.baseUrl + "/publications/get-list",
      paginatedRequest
    );
  }

  delete(id: string): Observable<any> {
    return this.http.delete(this.baseUrl + `/publications/${id}`);
  }

  getPublicationExternalData(
    paginatedRequest: PaginatedRequest,
    forUser: string
  ): Observable<PaginatedResponse<PublicationExternal>> {
    return this.http.post<PaginatedResponse<PublicationExternal>>(
      this.baseUrl + `/publications-external/get-list/${forUser}`,
      paginatedRequest
    );
  }

  getDashboardData(
    paginatedRequest: PaginatedRequest,
  ): Observable<PaginatedResponse<PublicationExternal>> {
    return this.http.post<PaginatedResponse<PublicationExternal>>(
      this.baseUrl + `/publications-external/get-dashboard-list`,
      paginatedRequest
    );
  }

  postComment(
    publicationId: string,
    comment: PostedComment
  ): Observable<PostedComment> {
    return this.http.post(
      this.baseUrl + `/publications/post-comment/${publicationId}`,
      comment
    ) as Observable<PostedComment>;
  }

  deleteComment(id: string): Observable<PostedComment> {
    return this.http.delete(
      this.baseUrl + `/publications/comments/${id}`
    ) as Observable<PostedComment>;
  }

  likePost(id: string): Observable<string> {
    return this.http.get(
      this.baseUrl + `/publications-external/like/${id}`
    ) as Observable<string>;
  }

  unlikePost(id: string): Observable<any> {
    return this.http.get(
      this.baseUrl + `/publications-external/unlike/${id}`
    ) as Observable<any>;
  }
}
