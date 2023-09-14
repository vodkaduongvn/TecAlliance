import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeCreateComponent } from './employee-create.component';
import { EmployeeService } from '../services/employee.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('EmployeeCreateComponent', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeeCreateComponent],
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
