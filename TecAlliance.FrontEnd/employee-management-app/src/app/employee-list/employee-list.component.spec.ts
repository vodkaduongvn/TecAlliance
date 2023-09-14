import { TestBed } from '@angular/core/testing';

import { EmployeeListComponent } from './employee-list.component';
import { EmployeeService } from '../services/employee.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('EmployeeListComponent', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;

 
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeeListComponent],
      imports: [HttpClientTestingModule],
      providers: [EmployeeService]
    });
    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(service).toBeTruthy();
  });
});
