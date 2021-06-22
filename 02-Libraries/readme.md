# Workshop 2 - Links and code snippets
### Slide 6
https://www.npmjs.com

### Slide 7
https://material.angular.io/guide/getting-started
```
ng add @angular/material
```

### Slide 8
https://material.angular.io/components/categories

### Slide 10
https://material.angular.io/components/toolbar/overview

app-navigation.component.html
```
<mat-toolbar>
  <mat-toolbar-row>
    <a [routerLink]="['/']">Home</a> |
    <a [routerLink]="['/todo']">Todo Items</a> |
    {{ settings.environment }} |
    {{ settings.apiUrl }}
  </mat-toolbar-row>
</mat-toolbar>
```

### Slide 12
https://material.angular.io/components/table/overview

todo-list.component.html:
```
<table mat-table [dataSource]="items" class="todo-list-datatable">
  <!-- id Column -->
  <ng-container matColumnDef="id">
    <th mat-header-cell *matHeaderCellDef> ID </th>
    <td mat-cell *matCellDef="let row"> {{row.id}} </td>
  </ng-container>

  <!-- name Column -->
  <ng-container matColumnDef="name">
    <th mat-header-cell *matHeaderCellDef> Name </th>
    <td mat-cell *matCellDef="let row"> {{row.name}} </td>
  </ng-container>

  <!-- description Column -->
  <ng-container matColumnDef="description">
    <th mat-header-cell *matHeaderCellDef> Description </th>
    <td mat-cell *matCellDef="let row"> {{row.description}} </td>
  </ng-container>

  <!-- createDate Column -->
  <ng-container matColumnDef="createDate">
    <th mat-header-cell *matHeaderCellDef> Created On </th>
    <td mat-cell *matCellDef="let row">{{row.createDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- dueDate Column -->
  <ng-container matColumnDef="dueDate">
    <th mat-header-cell *matHeaderCellDef> Due Date </th>
    <td mat-cell *matCellDef="let row">{{row.dueDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- completedDate Column -->
  <ng-container matColumnDef="completedDate">
    <th mat-header-cell *matHeaderCellDef> Completed On </th>
    <td mat-cell *matCellDef="let row"> {{row.completedDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- isLate Column -->
  <ng-container matColumnDef="isLate">
    <th mat-header-cell *matHeaderCellDef> Is Late </th>
    <td mat-cell *matCellDef="let row"> {{row.isLate}} </td>
  </ng-container>

  <!-- isPastDue Column -->
  <ng-container matColumnDef="isPastDue">
    <th mat-header-cell *matHeaderCellDef> Is Past Due </th>
    <td mat-cell *matCellDef="let row"> {{row.isPastDue}} </td>
  </ng-container>

  <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
  <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
</table>

<div class="todo-list-items" *ngIf="items">Found {{ items.length }} items.</div>
```
### Slide 13
todo-list.component.ts:
```
displayedColumns: string[] = ['id', 'name', 'description', 'createDate', 'dueDate', 'completedDate', 'isLate', 'isPastDue'];
```
### Slide 14
todo-list.component.scss
```
table.todo-list-datatable {
  width: 100%;

  .mat-column-id {
    width: 50px;
  }

  th.date-col,
  td.date-col {
    width: 150px;
  }

  th.bool-col,
  td.bool-col {
    width: 125px;
  }
}
```
### Slide 15
todo-list.component.scss
```
  th.mat-header-cell {
    background-color: #404040;
    color:white;
  }

  tr.mat-row:hover {
    background-color: #cce6ff;
  }

  tr.altRowStyle {
    background-color: #f5f5f5;
  }
```
todo-list.component.html
```
  <tr mat-row *matRowDef="let row; let oddRow = odd; columns: displayedColumns;"
    [ngClass]="{altRowStyle:oddRow}"></tr>
```
### Slide 16
todo-list.component.html
```
  <tr mat-row *matRowDef="let row; let oddRow = odd; columns: displayedColumns;"
    [ngClass]="{altRowStyle:oddRow}" (click)="itemClicked(row)"></tr>

```
todo-list.component.ts
```
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  constructor(private router: Router, private todoDataSvc: TodoDataService) { }

  displayedColumns: string[] = ['id', 'name', 'description', 'createDate', 'dueDate', 'completedDate', 'isLate', 'isPastDue'];
  items: Array<TodoItem>;

  ngOnInit() {
    this.todoDataSvc.getAll().subscribe(data => {
      this.items = data;
    });
  }

  public itemClicked(row: TodoItem) {
    this.router.navigate(['/todo', row.id]);
  }
}
```
### Slide 18
todo-list.component.ts
```
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

```
### Slide 20
todo-list.component.html
```
<table mat-table matSort [dataSource]="items" class="todo-list-datatable">
  <!-- id Column -->
  <ng-container matColumnDef="id">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> ID </th>
    <td mat-cell *matCellDef="let row"> {{row.id}} </td>
  </ng-container>

  <!-- name Column -->
  <ng-container matColumnDef="name">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Name </th>
    <td mat-cell *matCellDef="let row"> {{row.name}} </td>
  </ng-container>

  <!-- description Column -->
  <ng-container matColumnDef="description">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Description </th>
    <td mat-cell *matCellDef="let row"> {{row.description}} </td>
  </ng-container>

  <!-- createDate Column -->
  <ng-container matColumnDef="createDate">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Created On </th>
    <td mat-cell *matCellDef="let row">{{row.createDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- dueDate Column -->
  <ng-container matColumnDef="dueDate">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Due Date </th>
    <td mat-cell *matCellDef="let row">{{row.dueDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- completedDate Column -->
  <ng-container matColumnDef="completedDate">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Completed On </th>
    <td mat-cell *matCellDef="let row"> {{row.completedDate | date:'MM/dd/yyyy'}} </td>
  </ng-container>

  <!-- isLate Column -->
  <ng-container matColumnDef="isLate">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Is Late </th>
    <td mat-cell *matCellDef="let row"> {{row.isLate}} </td>
  </ng-container>

  <!-- isPastDue Column -->
  <ng-container matColumnDef="isPastDue">
    <th mat-header-cell *matHeaderCellDef mat-sort-header> Is Past Due </th>
    <td mat-cell *matCellDef="let row"> {{row.isPastDue}} </td>
  </ng-container>

  <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
  <tr mat-row *matRowDef="let row; let oddRow = odd; columns: displayedColumns;"
    [ngClass]="{altRowStyle:oddRow}" (click)="itemClicked(row)"></tr>
</table>

<div class="todo-list-items" *ngIf="items">Found {{ items.data.length }} items.</div>
```