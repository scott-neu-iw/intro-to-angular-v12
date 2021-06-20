import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss']
})
export class TodoItemComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private todoDataSvc: TodoDataService
  ) {}

  public itemId!: number;
  public item!: TodoItem;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.itemId = params.id;

      this.todoDataSvc.get(this.itemId).subscribe(data => {
        this.item = data;
      });
    });
  }
}
