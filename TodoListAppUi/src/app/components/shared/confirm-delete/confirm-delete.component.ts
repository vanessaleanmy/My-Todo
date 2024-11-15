import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-delete',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirm-delete.component.html',
  styleUrl: './confirm-delete.component.scss'
})
export class ConfirmDeleteComponent {
  @Input() itemName: string = '';
  @Output() confirmed = new EventEmitter<boolean>();
  isVisible: boolean = true;  

  confirmDelete(): void {
    this.isVisible = false;
    this.confirmed.emit(true);
  }

  cancelDelete(): void {
    this.isVisible = false;
    this.confirmed.emit(false);
  }
}
