import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodoModel } from '../models/todo.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private get apiUrl(): string { return `${environment.baseApiUrl}/api/todo`}  
  
  constructor(private http:HttpClient) { }

  getTodos(): Observable<TodoModel[]>{
    return this.http.get<TodoModel[]>(this.apiUrl);
  }

  addTodo(item: TodoModel): Observable<TodoModel>{  
    return this.http.post<TodoModel>(this.apiUrl, item);
  }

  deleteTodo(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateTodo(id:string, isDone: boolean):Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/completed`,isDone);
  }

}
