import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  constructor(private router: Router, private todoDataSvc: TodoDataService) { }

  public displayedColumns: string[] = ['id', 'name', 'description', 'createDate', 'dueDate', 'completedDate', 'isLate', 'isPastDue'];
  public items: MatTableDataSource<TodoItem> = new MatTableDataSource<TodoItem>();

  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.todoDataSvc.getAll().subscribe(data => {
      this.items = new MatTableDataSource(data);
      this.items.sort = this.sort;
    });
  }

  public itemClicked(row: TodoItem) {
    this.router.navigate(['/todo', row.id]);
  }
}
