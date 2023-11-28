import { Component, Inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatChipInputEvent } from "@angular/material/chips";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Publication } from "src/app/shared/models/publication.interface";
import { PublicationService } from "src/app/shared/services/publication.service";

@Component({
  selector: "app-edit-post",
  templateUrl: "./edit-post.component.html",
  styleUrls: ["./edit-post.component.scss"],
})
export class EditPostComponent implements OnInit {
  errors: string;

  ngOnInit() {}

  labels: Array<string> = [];
  form = {
    title: "",
    labels: "",
    body: "",
    id: "",
  } as Publication;
  publicationForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditPostComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Publication,
    private publicationService: PublicationService
  ) {
    if (data != null) {
      this.form = data;
      this.labels = data.labels.split(",");
    }
    this.initForm(this.form);
  }

  initForm(form: Publication) {
    this.publicationForm = this.formBuilder.group({
      title: [form.title, Validators.required],
      body: [form.body, Validators.required],
    });
  }

  removeKeyword(keyword: string) {
    const index = this.labels.indexOf(keyword);
    if (index >= 0) {
      this.labels.splice(index, 1);
    }
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || "").trim();

    // Add our keyword
    if (value) {
      this.labels.push(value);
    }

    // Clear the input value
    event.chipInput.inputElement.value = "";
  }

  submit(form: FormGroup) {
    this.errors = null;
    var combinedLabels = "";

    this.labels.forEach((element: string) => {
      combinedLabels += `${element},`;
    });
    combinedLabels = combinedLabels.slice(0, combinedLabels.length - 1);

    var publication = {
      id: this.form.id,
      title: form.get("title").value,
      body: form.get("body").value,
      labels: combinedLabels,
    } as Publication;

    this.publicationService.upsertPublication(publication).subscribe({
      next: (_) => {
        this.closeWindow(true);
      },
      error: (error: any) => {
        this.errors = error.error;
      },
    });
  }

  closeWindow(refresh: boolean = false) {
    this.dialogRef.close({dialogResult: refresh});
  }
}
