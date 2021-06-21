import { Component, OnInit } from '@angular/core';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  constructor(private todoDataSvc: TodoDataService) { }

  public items: Array<TodoItem> = [];

  ngOnInit() {
    this.todoDataSvc.getAll().subscribe(data => {
      this.items = data;
    });
  }
}
