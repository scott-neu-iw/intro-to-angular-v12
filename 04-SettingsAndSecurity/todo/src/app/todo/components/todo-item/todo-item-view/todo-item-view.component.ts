import { Component, OnInit, Input } from '@angular/core';
import { TodoItem } from '../../../models/todo-item.model';

@Component({
  selector: 'app-todo-item-view',
  templateUrl: './todo-item-view.component.html',
  styleUrls: ['./todo-item-view.component.scss']
})
export class TodoItemViewComponent implements OnInit {
  constructor() { }

  @Input() item!: TodoItem;

  ngOnInit(): void {
  }

}
