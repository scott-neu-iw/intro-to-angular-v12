import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private todoDataSvc: TodoDataService,
    private snackBar: MatSnackBar
  ) {}

  public itemId!: number;
  public item!: TodoItem;
  public isEditMode = false;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.itemId = Number(params.id);

      if (this.itemId === 0) {
        this.item = {
          id: 0,
          name: '',
          description: '',
          assignedTo: '',
          completed: false,
          isLate: false,
          isPastDue: false,
          completedBy: '',
          createDate: new Date(),
        };
        this.isEditMode = true;
      } else {
        this.todoDataSvc.get(this.itemId).subscribe(data => {
          this.item = data;
        });
      }
    });
  }

  public edit() {
    this.isEditMode = true;
  }

  public cancel() {
    this.isEditMode = false;
    if (this.itemId === 0) {
      this.router.navigate(['/todo']);
    }
  }

  public delete() {
    this.todoDataSvc.delete(this.itemId).subscribe(() => {
      this.snackBar.open("Success: Item deleted!", "Close", { duration: 5*1000 })
      this.router.navigate(['/todo']);
    });
  }

  public save(value: TodoItem) {
    // update the route on a new item save, without triggering a route change
    if (this.itemId === 0) {
      const url = this.router.createUrlTree(['/todo', value.id]).toString();
      this.location.go(url);
    }

    this.itemId = value.id;
    this.item = value;
    this.isEditMode = false;

    this.snackBar.open("Success: Item saved!", "Close", { duration: 5*1000 })
  }
}
