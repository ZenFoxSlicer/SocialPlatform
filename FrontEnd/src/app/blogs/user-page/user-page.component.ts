import {
  Component,
  ElementRef,
  Input,
  QueryList,
  ViewChildren,
} from "@angular/core";
import { MatExpansionPanel } from "@angular/material/expansion";
import { MatFormField } from "@angular/material/form-field";
import { MatInput } from "@angular/material/input";
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterEvent,
} from "@angular/router";
import { PostedComment } from "src/app/shared/models/comment.interface";
import { PaginatedRequest } from "src/app/shared/models/paginated-request.model";
import { PaginatedResponse } from "src/app/shared/models/paginated-response.model";
import { PublicationExternal } from "src/app/shared/models/publication-external.inteface";
import { Publication } from "src/app/shared/models/publication.interface";
import { PublicationService } from "src/app/shared/services/publication.service";
import { UserService } from "src/app/shared/services/user.service";
import * as crypto from "crypto-js";
import { UserInfo } from "src/app/shared/models/user-info.interface";
import { filter } from "rxjs";
import { MatMenu } from "@angular/material/menu";
import { MatMenuTrigger } from "@angular/material/menu";
import { FollowingService } from "src/app/shared/services/following.service";

@Component({
  selector: "app-user-page",
  templateUrl: "./user-page.component.html",
  styleUrls: ["./user-page.component.scss"],
})
export class UserPageComponent {
  @ViewChildren(MatExpansionPanel) panels: QueryList<MatExpansionPanel>;
  @ViewChildren("inputRefs") inputRefs: QueryList<MatFormField>;
  @ViewChildren("menu") menus: QueryList<MatMenuTrigger>;

  username: string;
  data: Array<PublicationExternal> = [];
  totalData: number = 0;
  paginatedRequest = {
    pageIndex: 0,
    pageSize: 10,
    searchString: "",
  } as PaginatedRequest;
  searchString: string;
  userInfo: UserInfo;
  isFollowing: boolean;
  currentUser: string;
  /**
   *
   */
  constructor(
    private publicationService: PublicationService,
    public userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private followingService: FollowingService
  ) {
    this.router.setUpLocationChangeListener();
    router.events
      .pipe(filter((events: RouterEvent) => events instanceof NavigationEnd))
      .subscribe((val) => {
        console.log(val);
        location.reload();
      });
  }
  ngOnInit() {
    this.currentUser = this.userService.isLoggedIn
      ? this.userService.getUserName()
      : "";
    this.username = this.route.snapshot.paramMap.get("username");
    this.loadData();
    this.userService
      .getBasicInfo(this.username)
      .subscribe((response: UserInfo) => {
        this.userInfo = response;
        this.getIsFollowing();
      });
    // this.route.paramMap.pipe(
    //   switchMap((params) => (this.username = String(params.get("username"))))
    // );
  }

  getIsFollowing() {
    if (this.userInfo.userName != this.currentUser) {
      this.followingService
        .isfollowing(this.userInfo.id)
        .subscribe((res: boolean) => (this.isFollowing = res));
    }
  }

  get profilePicBgColor() {
    return;
  }

  get isLoggedIn() {
    return this.userService.isLoggedIn();
  }

  openComments(index: number) {
    var result = this.panels.get(index).toggle();
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

  search() {
    this.paginatedRequest.searchString = this.searchString;
    this.paginatedRequest.pageIndex = 0;
    this.data = [];
    this.loadData();
  }

  loadData() {
    this.publicationService
      .getPublicationExternalData(this.paginatedRequest, this.username)
      .subscribe((result: PaginatedResponse<PublicationExternal>) => {
        this.data = this.data.concat(result.data);
        this.data.length;
        this.totalData = result.totalData;
      });
  }

  getLabels(item: PublicationExternal) {
    return item.labels.split(",");
  }

  seeMore() {
    this.paginatedRequest.pageIndex++;
    this.loadData();
  }

  get getProfileInitials() {
    return this.getInitials(
      `${this.userInfo.firstName} ${this.userInfo.lastName}`
    );
  }

  get getProfileColor() {
    return this.getColor(
      `${this.userInfo.firstName} ${this.userInfo.lastName}`
    );
  }

  getInitials(value: string) {
    var name = value.split(" ");
    return `${name[0].charAt(0)}${name[1].charAt(0)}`;
  }

  getColor(name: string) {
    const hash = crypto.SHA256(name);
    return `#${hash.toString().substring(0, 6)}`;
  }

  navigateToUser(userName: string) {
    this.router.navigate(["blogs", userName]).then(() => location.reload());
  }

  deleteComment(comment: PostedComment, index: number) {
    this.publicationService.deleteComment(comment.id).subscribe(() => {
      this.data
        .find((x) => x.id == comment.publicationId)
        .comments.splice(index, 1);
    });
  }

  canDeleteComment(comment: PostedComment) {
    return (
      this.username == this.currentUser ||
      comment.authorUserName == this.currentUser
    );
  }

  follow() {
    this.followingService
      .follow(this.userInfo.id)
      .subscribe(() => (this.isFollowing = true));
  }

  unfollow() {
    this.followingService
      .unfollow(this.userInfo.id)
      .subscribe(() => (this.isFollowing = false));
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
}
