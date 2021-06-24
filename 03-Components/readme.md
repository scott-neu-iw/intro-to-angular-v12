# Workshop 3 - Components and Forms
### Slide 9
todo-item-edit.component.ts
```
  @Input() item: TodoItem;
  @Output() cancelled = new EventEmitter<void>();
  @Output() saved = new EventEmitter<TodoItem>();

  ngOnInit() {
  }

  public cancel() {
    this.cancelled.emit();
  }
```
todo-item-edit.component.html
```
  <div>
    <button mat-stroked-button (click)="cancel()">Cancel</button>
  </div>
```
### Slide 10
```
  public edit() {
    this.isEditMode = true;
  }

  public cancel() {
    this.isEditMode = false;
  }

  public save(item: TodoItem) {
    // TODO: display save message
    this.isEditMode = false;
  }
```
### 2b - Slide 11
```
<app-todo-item-view *ngIf="!isEditMode" [item]="item"></app-todo-item-view>
<app-todo-item-edit *ngIf="isEditMode" [item]="item" (saved)="save($event)" (cancelled)="cancel()"></app-todo-item-edit>
<div *ngIf="!isEditMode">
  <button mat-stroked-button color="primary" (click)="edit()">Edit</button>
  <button mat-stroked-button [routerLink]="['/todo']">&lt; Back</button>
</div>
```
### 2b - Slide 13
https://angular.io/guide/forms-overview
### 2b - Slide 14
https://github.com/scott-neu-iw/intro-to-angular/tree/develop/Service

todo-item-edit.component.ts
```
<form>
  <div class="input-container">
    <mat-form-field>
      <input matInput placeholder="Name">
    </mat-form-field>
    <mat-form-field>
      <textarea matInput placeholder="Description"></textarea>
    </mat-form-field>
    <mat-form-field>
      <input matInput placeholder="Assignee">
    </mat-form-field>
    <mat-form-field>
      <input matInput [matDatepicker]="dueDate" placeholder="Due Date">
      <mat-datepicker-toggle matSuffix [for]="dueDate"></mat-datepicker-toggle>
      <mat-datepicker #dueDate></mat-datepicker>
    </mat-form-field>
  </div>
  <div>
    <button mat-stroked-button type="button" (click)="cancel()">Cancel</button>
  </div>
</form>
```
todo-item-edit.component.scss
```
.input-container {
  display: flex;
  flex-direction: column;
  width: 400px;
  padding: 25px 0 0 25px;
}

.input-container > * {
  width: 100%;
}
```
### Slide 15
todo-item-edit.html
```
<form #todoForm="ngForm">
  <div class="input-container">
    <mat-form-field>
      <input matInput placeholder="Name"
      id="name" name="name" [(ngModel)]="item.name">
    </mat-form-field>
    <mat-form-field>
      <textarea matInput placeholder="Description"
      id="description" name="description" [(ngModel)]="item.description"></textarea>
    </mat-form-field>
    <mat-form-field>
      <input matInput placeholder="Assignee"
      id="assignedTo" name="assignedTo" [(ngModel)]="item.assignedTo">
    </mat-form-field>
    <mat-form-field>
      <input matInput [matDatepicker]="dueDate"
      id="dueDate" name="dueDate" placeholder="Due Date" [(ngModel)]="item.dueDate">
      <mat-datepicker-toggle matSuffix [for]="dueDate"></mat-datepicker-toggle>
      <mat-datepicker #dueDate></mat-datepicker>
    </mat-form-field>
  </div>
  <div>
    <button mat-stroked-button type="button" (click)="cancel()">Cancel</button>
  </div>
</form>
```
### Slide 17
```
name.className = {{ nameSpy.className }}
<br>
name.validity.valid = {{ nameSpy.validity.valid }}
<br>
name.validity.valueMissing = {{ nameSpy.validity.valueMissing }}
<br>
form.valid = {{ todoForm.form.valid }}
<br>
form.touched = {{ todoForm.form.touched }}
<br>
form.untouched = {{ todoForm.form.untouched }}
<br>
form.pristine = {{ todoForm.form.pristine }}

```
### Slide 20
todo-item-save.model.ts
```
export interface TodoItemSave {
  id: number;
  name: string;
  description: string;
  dueDate?: Date;
  assignedTo: string;
}
```
todo-item-edit.component.ts
```
model: TodoItemSave;
@Input()
set item(value: TodoItem) {
   this.model = this.mapToSaveItem(value);
}
```
```
private mapToSaveItem(value: TodoItem): TodoItemSave {
return {
    id: value.id,
    name: value.name,
    description: value.description,
    dueDate: value.dueDate,
    assignedTo: value.assignedTo,
  };
}
```
### Slide 21
```
<button mat-stroked-button type="submit" color="primary"
  [disabled]="!todoForm.form.valid">Save</button>
```
### Slide 22
todo-data.service.ts
```
public add(item: TodoItemSave): Observable<TodoItem> {
  return this.httpClient.post<TodoItem>(this.baseUrl, item);
}

public update(id: number, item: TodoItemSave): Observable<TodoItem> {
    const url = `${this.baseUrl}/${id}`;
return this.httpClient.put<TodoItem>(url, item);
}

```
### Slide 23
todo-item-edit.component.ts
```
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
```
