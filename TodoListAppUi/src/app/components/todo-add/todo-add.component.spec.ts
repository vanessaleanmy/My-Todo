import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoAddComponent } from './todo-add.component';


describe('TodoAddComponent', () => {
  let component: TodoAddComponent;
  let fixture: ComponentFixture<TodoAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have an invalid form when empty', () => {
    const formElement = fixture.nativeElement.querySelector('form');
    expect(formElement).toBeTruthy();
    expect(component.newTodoItem).toBe('');
    expect(formElement.checkValidity()).toBeFalsy();
  });  
  
});
