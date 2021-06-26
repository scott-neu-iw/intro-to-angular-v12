import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedInGuard } from '../core/guards/is-logged-in.guard';

import { TodoItemComponent } from './components/todo-item/todo-item.component';
import { TodoListComponent } from './components/todo-list/todo-list.component';

const routes: Routes = [
  {
    path: 'todo',
    children: [
      {
        path: '',
        component: TodoListComponent,
        canActivate: [IsLoggedInGuard]
      },
      {
        path:':id',
        component: TodoItemComponent,
        canActivate: [IsLoggedInGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TodoRoutingModule { }
