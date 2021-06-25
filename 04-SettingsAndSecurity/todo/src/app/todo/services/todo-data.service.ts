import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AppSettingsService } from '../../core/services/app-settings.service';
import { TodoItem } from '../models/todo-item.model';
import { TodoItemSave } from '../models/todo-item-save.model';

@Injectable({
  providedIn: 'root'
})
export class TodoDataService {
  constructor(private appSettingsSvc: AppSettingsService,
    private httpClient: HttpClient) { }

  private baseUrl = `${this.appSettingsSvc.settings.apiUrl}/v1/todoitems`;

  public getAll(): Observable<Array<TodoItem>> {
    return this.httpClient.get<Array<TodoItem>>(this.baseUrl);
  }

  public get(id: number): Observable<TodoItem> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<TodoItem>(url);
  }

  public add(item: TodoItemSave): Observable<TodoItem> {
    return this.httpClient.post<TodoItem>(this.baseUrl, item);
  }

  public update(id: number, item: TodoItemSave): Observable<TodoItem> {
      const url = `${this.baseUrl}/${id}`;
  return this.httpClient.put<TodoItem>(url, item);
  }

  public delete(id: number): Observable<object> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.delete(url);
  }
}
