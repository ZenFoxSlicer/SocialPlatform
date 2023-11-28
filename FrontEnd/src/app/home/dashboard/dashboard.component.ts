import { Component, OnInit, QueryList, ViewChildren } from "@angular/core";
import * as crypto from "crypto-js";
import { PaginatedRequest } from "src/app/shared/models/paginated-request.model.js";
import { PublicationExternal } from "src/app/shared/models/publication-external.inteface.js";
import { PaginatedResponse } from "src/app/shared/models/paginated-response.model.js";
import { PublicationService } from "src/app/shared/services/publication.service";
import { PostedComment } from "src/app/shared/models/comment.interface";
import { MatFormField } from "@angular/material/form-field";
import { UserService } from "src/app/shared/services/user.service";
import { Router } from "@angular/router";
import { MatExpansionPanel } from "@angular/material/expansion";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent implements OnInit {
  @ViewChildren("inputRefs") inputRefs: QueryList<MatFormField>;
  @ViewChildren(MatExpansionPanel) panels: QueryList<MatExpansionPanel>;

  paginatedRequest = {
    pageIndex: 0,
    pageSize: 10,
    searchString: "",
  } as PaginatedRequest;
  data: Array<PublicationExternal> = [];
  totalData: number =0;
  currentUser: any;

  constructor(
    private publicationService: PublicationService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit() {
    this.currentUser = this.userService.isLoggedIn
      ? this.userService.getUserName()
      : "";
    this.loadData();
  }
  loadData() {
    this.publicationService
      .getDashboardData(this.paginatedRequest)
      .subscribe((result: PaginatedResponse<PublicationExternal>) => {
        this.data = this.data.concat(result.data);
        this.data.length;
        this.totalData = result.totalData;
      });
  }

  getCommentInitials(value: string) {
    var name = value.split(" ");
    return `${name[0].charAt(0)}${name[1].charAt(0)}`;
  }

  getInitials(firstName: string, lastName: string) {
    return `${firstName.charAt(0)}${lastName.charAt(0)}`;
  }

  getColor(name: string) {
    const hash = crypto.SHA256(name);
    return `#${hash.toString().substring(0, 6)}`;
  }

  postComment(index: number) {
    var input = this.inputRefs.get(index);
    var value = input._control.value;
    var publication = this.data[index];
    var comment = {
      body: this.inputRefs.get(index)._control.value,
    } as PostedComment;

    this.publicationService
      .postComment(publication.id, comment)
      .subscribe((response: PostedComment) => {
        publication.comments.push(response);
        input._control.value = "";
      });
  }

  deleteComment(comment: PostedComment, index: number) {
    this.publicationService.deleteComment(comment.id).subscribe(() => {
      this.data
        .find((x) => x.id == comment.publicationId)
        .comments.splice(index, 1);
    });
  }

  canDeleteComment(comment: PostedComment) {
    var publication = this.data.find(
      (publication: PublicationExternal) =>
        publication.id == comment.publicationId
    );

    return (
      publication.author.userName == this.currentUser ||
      comment.authorUserName == this.currentUser
    );
  }

  navigateToUser(userName: string) {
    this.router.navigate(["blogs", userName]).then(() => location.reload());
  }

  openComments(index: number) {
    var result = this.panels.get(index).toggle();
  }

  likePost(item: PublicationExternal) {
    this.publicationService.likePost(item.id).subscribe((response: string) => {
      item.likes.unshift(response);
      item.isLikedByCurrentUser = true;
    });
  }

  unlikePost(item: PublicationExternal) {
    this.publicationService
      .unlikePost(item.id)
      .subscribe((response: string) => {
        item.likes.splice(
          item.likes.findIndex((like: string) => like == response),
          1
        );
        item.isLikedByCurrentUser = false;
      });
  }

  seeMore() {
    this.paginatedRequest.pageIndex++;
    this.loadData();
  }
}
