import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TodoItemSave } from '../../../models/todo-item-save.model';
import { TodoItem } from '../../../models/todo-item.model';
import { TodoDataService } from '../../../services/todo-data.service';

@Component({
  selector: 'app-todo-item-edit',
  templateUrl: './todo-item-edit.component.html',
  styleUrls: ['./todo-item-edit.component.scss']
})
export class TodoItemEditComponent implements OnInit {
  constructor(private todoDataSvc: TodoDataService) { }

  public model!: TodoItemSave;
  @Input()
  set item(value: TodoItem) {
     this.model = this.mapToSaveItem(value);
  }
  @Output() cancelled = new EventEmitter<void>();
  @Output() saved = new EventEmitter<TodoItem>();

  ngOnInit() {
  }

  public cancel() {
    this.cancelled.emit();
  }

  public submit() {
    if (this.model.id === 0) {
      this.todoDataSvc.add(this.model).subscribe(data => {
        this.saved.emit(data);
      });
    } else {
      this.todoDataSvc.update(this.model.id, this.model).subscribe(data => {
        this.saved.emit(data);
      });
    }
  }

  private mapToSaveItem(value: TodoItem): TodoItemSave {
    return {
        id: value.id,
        name: value.name,
        description: value.description,
        dueDate: value.dueDate,
        assignedTo: value.assignedTo,
      };
    }
}
