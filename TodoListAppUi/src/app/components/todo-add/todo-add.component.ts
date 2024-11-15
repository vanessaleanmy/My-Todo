import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-todo-add',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './todo-add.component.html',
  styleUrl: './todo-add.component.scss'
})
export class TodoAddComponent {
  @Input() existingTodos: string[] = []; 
  @Output() todoAdded = new EventEmitter<string>();
  newTodoItem: string = '';
   duplicateError = false;

  onSubmit(todoForm: NgForm) {
    this.checkDuplicate();
    if (!this.duplicateError) {
      this.todoAdded.emit(this.newTodoItem.trim());
      this.newTodoItem = ''; 
      todoForm.resetForm();
    }
  }

  checkDuplicate() {
    const trimmedTodo = this.newTodoItem.trim();
    this.duplicateError = this.existingTodos.includes(trimmedTodo.toLowerCase());
  }
}
