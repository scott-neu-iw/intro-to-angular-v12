# Workshop 1 - Links and code snippets
### Slide 22:
app-settings.model.ts
```
export interface AppSettings {
  environment: string;
  apiUrl: string;
}
```
### Slide 23:
app-settings.service.ts
```
import { Injectable } from '@angular/core';
import { AppSettings } from '../models/app-settings.model';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  constructor() {
    this.appSettings = {
      environment: 'dev',
      apiUrl: 'http://localhost:8080'
    };
  }

  private appSettings: AppSettings;
  get settings(): AppSettings {
    return this.appSettings;
  }
}
```
### Slide 26:
app-navigation.component.ts
```
import { Component, OnInit } from '@angular/core';
import { AppSettingsService } from '../../core/services/app-settings.service';
import { AppSettings } from '../../core/models/app-settings.model';

@Component({
  selector: 'app-app-navigation',
  templateUrl: './app-navigation.component.html',
  styleUrls: ['./app-navigation.component.scss']
})
export class AppNavigationComponent implements OnInit {
  constructor(private appSettingsSvc: AppSettingsService) { }

  public settings!: AppSettings;

  ngOnInit() {
    this.settings = this.appSettingsSvc.settings;
  }
}
```
app-navigation.component.html
```
<div>
  {{ settings.environment }} | {{ settings.apiUrl }}
</div>
<hr>
```
### Slide 29:
https://github.com/scott-neu-iw/intro-to-angular-v12/tree/main/Service

### Slide 31:
todo-item.model.ts
```
export interface TodoItem {
  id: number;
  name: string;
  description: string;
  createDate: Date;
  dueDate?: Date;
  assignedTo: string;
  completedDate?: Date;
  completedBy: string;
  completed: boolean;
  isPastDue: boolean;
  isLate: boolean;
}
```
### Slide 32:
todo-data.service.ts
```
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AppSettingsService } from '../../core/services/app-settings.service';
import { TodoItem } from '../models/todo-item.model';

@Injectable({
  providedIn: 'root'
})
export class TodoDataService {
  constructor(private appSettingsSvc: AppSettingsService, private httpClient: HttpClient) { }

  private baseUrl = `${this.appSettingsSvc.settings.apiUrl}/v1/todoitems`;

  public getAll(): Observable<Array<TodoItem>> {
    return this.httpClient.get<Array<TodoItem>>(this.baseUrl);
  }

  public get(id: number): Observable<TodoItem> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<TodoItem>(url);
  }
}

```
### Slide: 36
todo-routing.module.ts
```
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TodoListComponent } from './components/todo-list/todo-list.component';

const routes: Routes = [
  {
    path: 'todo',
    children: [
      {
        path: '',
        component: TodoListComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TodoRoutingModule { }
```
app-navigation.component.html
```
<div>
  {{ settings.environment }} | {{ settings.apiUrl }}
</div>
<a [routerLink]="['/todo']">To Do List</a>
<hr>
```

### Slide 37:
todo-list.component.ts
```
import { Component, OnInit } from '@angular/core';
import { TodoDataService } from '../../services/todo-data.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  constructor(private todoDataSvc: TodoDataService) { }

  items!: Array<TodoItem>;

  ngOnInit() {
    this.todoDataSvc.getAll().subscribe(data => {
      this.items = data;
    });
  }
}
```
todo-list.component.html
```
<div *ngIf="items">Found {{ items.length }}</div>
<ul>
  <li *ngFor="let item of items">
    {{ item.name }} -
    {{ item.description }} -
    {{ item.createDate | date:'MM/dd/yyy' }}
  </li>
</ul>
```
### Slide 40
todo-item.component.ts
```
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

  itemId!: number;
  item!: TodoItem;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.itemId = params.id;

      this.todoDataSvc.get(this.itemId).subscribe(data => {
        this.item = data;
      });
    });
  }
}
```
todo-item.component.html
```
<div><a [routerLink]="['/todo']" >Back</a></div>
<div *ngIf="item">
  {{ item | json }}
</div>
```