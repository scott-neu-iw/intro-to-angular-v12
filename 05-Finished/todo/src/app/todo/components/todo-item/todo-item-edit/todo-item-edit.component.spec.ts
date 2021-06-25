import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoItemEditComponent } from './todo-item-edit.component';

describe('TodoItemEditComponent', () => {
  let component: TodoItemEditComponent;
  let fixture: ComponentFixture<TodoItemEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodoItemEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TodoItemEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
