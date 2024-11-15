import { TestBed } from '@angular/core/testing';

import { TodoService } from './todo.service';
import { provideHttpClient } from '@angular/common/http';

describe('TodoService', () => {
  let service: TodoService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[provideHttpClient()]
    });
    service = TestBed.inject(TodoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
