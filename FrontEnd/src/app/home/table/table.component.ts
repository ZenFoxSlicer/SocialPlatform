import { Component, OnInit, ViewChild } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { Employee } from '../../shared/models/employee.model';
import { TableService } from '../../shared/services/table.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent implements OnInit {

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['position', 'name', 'yearsWorked', 'age', 'actions'];
  dataSource: MatTableDataSource<Employee>;
  selection = new SelectionModel<Employee>(true, []);
  selectedRows: Array<Employee> = [];
  errors: any;

  constructor(
    private tableService: TableService
  ) { }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.tableService.getData().subscribe(
      res => {
        this.dataSource = new MatTableDataSource<Employee>(res);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      },
      error => console.log(error)
    );
  }

  delete(name: string) {
    this.tableService.deleteEmployee(name).subscribe(
      res => { this.getData() },
      err => { this.errors = err.message }
    );
  }

  applyFilter(event){}
}
