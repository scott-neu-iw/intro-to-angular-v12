import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
    private todoDataSvc: TodoDataService,
    private snackBar: MatSnackBar
  ) {}

  public itemId!: number;
  public item!: TodoItem;
  public isEditMode = false;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.itemId = params.id;

      this.todoDataSvc.get(this.itemId).subscribe(data => {
        this.item = data;
      });
    });
  }

  public edit() {
    this.isEditMode = true;
  }

  public cancel() {
    this.isEditMode = false;
  }

  public save(value: TodoItem) {
    this.itemId = value.id;
    this.item = value;
    this.isEditMode = false;

    this.snackBar.open("Success: Item saved!", "Close", { duration: 5*1000 })
  }
}
