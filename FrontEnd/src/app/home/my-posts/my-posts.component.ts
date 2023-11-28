import { Component, OnInit, ViewChild } from "@angular/core";
import { EditPostComponent } from "./edit-post/edit-post.component";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { PublicationService } from "src/app/shared/services/publication.service";
import { PaginatedRequest } from "src/app/shared/models/paginated-request.model";
import { PaginatedResponse } from "src/app/shared/models/paginated-response.model";
import { Publication } from "src/app/shared/models/publication.interface";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: "app-my-posts",
  templateUrl: "./my-posts.component.html",
  styleUrls: ["./my-posts.component.scss"],
})
export class MyPostsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  data: any[] = [];
  constructor(
    public dialog: MatDialog,
    private publicationService: PublicationService,
    private snackBar: MatSnackBar
  ) {}
  pageSizeOptions = [10, 50, 100];
  searchString: string;
  paginatedRequest = {
    pageIndex: 0,
    pageSize: this.pageSizeOptions[0],
    searchString: "",
  } as PaginatedRequest;

  paginatedResponse = {
    totalData: 0,
    data: [],
  } as PaginatedResponse<Publication>;

  postDialogConfig: MatDialogConfig = {
    minHeight: "100%",
    minWidth: "60%",
    position: {
      top: "30vh",
    },
    disableClose: true,
  };

  ngAfterViewInit() {
    this.paginator.pageSizeOptions = this.pageSizeOptions;
    this.paginator.pageSize = this.pageSizeOptions[0];
    this.loadData();
    this.paginator.page.subscribe((event: PageEvent) => {
      this.paginatedRequest.pageIndex = event.pageIndex;
      this.paginatedRequest.pageSize = event.pageSize;
      this.loadData();
    });
  }

  items: Array<number> = [1, 2, 3];
  ngOnInit() {}


  openAddPost() {
    this.postDialogConfig.data = null;
    var ref = this.dialog.open(EditPostComponent, this.postDialogConfig);
    ref.afterClosed().subscribe((event: any) => {
      if (event.dialogResult) {
        this.loadData();
      }
    });
  }

  getLabels(item: Publication) {
    return item.labels.split(",");
  }

  editPublication(item: Publication) {
    this.postDialogConfig.data = item;
    var ref = this.dialog.open(EditPostComponent, this.postDialogConfig);
    ref.afterClosed().subscribe((event: any) => {
      if (event.dialogResult) {
        this.loadData();
      }
    });
  }

  loadData() {
    this.publicationService
      .getData(this.paginatedRequest)
      .subscribe((result: PaginatedResponse<Publication>) => {
        this.data = result.data;
        this.paginator.length = result.totalData;
      });
  }

  search() {
    this.paginatedRequest.searchString = this.searchString;
    this.paginatedRequest.pageIndex =  this.paginator.pageIndex = 0;
    this.loadData();
  }

  delete(item: string, index: number) {
    this.publicationService.delete(item).subscribe(
      () => this.data.splice(index, 1),
      (error: any) => this.openSnackBar(error.message)
    );
  }

  openSnackBar(message: string) {
    this.snackBar.open(message, "Close", {
      horizontalPosition: "end",
      verticalPosition: "bottom",
      duration: 5000
    });
  }
}
