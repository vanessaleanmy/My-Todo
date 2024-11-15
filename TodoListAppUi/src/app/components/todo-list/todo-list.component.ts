import { Component, OnInit } from '@angular/core';
import { TodoModel } from '../../models/todo.model';
import { CommonModule } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import { MatCheckbox } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { TodoService } from '../../services/todo.service';
import { ConfirmDeleteComponent } from '../shared/confirm-delete/confirm-delete.component';
import { TodoAddComponent } from '../todo-add/todo-add.component';


@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule,ConfirmDeleteComponent,TodoAddComponent,FormsModule,MatIconModule,MatCheckbox],
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.scss'
})
export class TodoListComponent implements OnInit{
  //newTodoItem: string ='';
  newTodoModel: TodoModel = {id:'',item:'',isDone: false};
  todos: TodoModel[] =[];
  itemToDelete: TodoModel | null = null; 
  
  
  constructor(private todoService: TodoService){}

  ngOnInit(): void {
    this.getAllTodo();
  }
  get existingTodoItems(): string[] {
    return this.todos.map(todo => todo?.item.toLowerCase() || '');
  }

  getAllTodo(){
    this.todoService.getTodos().subscribe({
          next: (todoList)=>{
            this.todos = todoList;
          },
          error: (response)=>{
            console.error('An error occurred in getAllTodo:', response);    
          }
        });
  }

  onCheckboxChange(todo:TodoModel){
    this.todoService.updateTodo(todo.id,todo.isDone).subscribe();        
  }
  
  addTodo(newTodoItem:string): void {
    const item = newTodoItem.trim();
    if (item) {    
      this.newTodoModel = {id:'00000000-0000-0000-0000-000000000000',item: item ,isDone: false};
      this.todoService.addTodo(this.newTodoModel).subscribe({
        next: ()=>{
          //this.newTodoItem = '';        
          this.getAllTodo();
        }
      });        
    }
  }

  promptDelete(todo: TodoModel): void {
    this.itemToDelete = todo;        
  }

  confirmDelete(confirmed: boolean): void {
    if (confirmed && this.itemToDelete) {
      // Proceed with delete if confirmed
      this.todoService.deleteTodo(this.itemToDelete.id).subscribe(() => {
        this.todos = this.todos.filter(t => t.id !== this.itemToDelete!.id);
        this.itemToDelete = null; // Reset the itemToDelete
      });
    } else {
      // Cancel delete
      this.itemToDelete = null;
    }
  }

}

  
