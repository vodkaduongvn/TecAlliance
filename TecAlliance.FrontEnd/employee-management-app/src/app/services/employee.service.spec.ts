import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeService } from './employee.service';
import { Employee } from '../models/employee.model';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;

  const apiUrl = 'https://localhost:7040/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeService]
    });

    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve employees from the API via GET', () => {
    const mockEmployees: Employee[] = [
      { id: 1, name: 'John', position: 'Developer', hiringDate: '2023-09-12', salary: 50000 },
      { id: 2, name: 'Jane', position: 'Designer', hiringDate: '2023-09-13', salary: 60000 }
    ];

    service.getEmployees().subscribe((employees: Employee[]) => {
      expect(employees).toEqual(mockEmployees);
    });

    const request = httpMock.expectOne(`${apiUrl}/employees`);
    expect(request.request.method).toBe('GET');
    request.flush(mockEmployees);
  });

  it('should retrieve an employee by id from the API via GET', () => {
    const dummyEmployee: Employee = { id: 1, name: 'John', position: 'Developer', hiringDate: '2023-09-12', salary: 50000 };

    service.getEmployeeById(1).subscribe((employee) => {
      expect(employee).toEqual(dummyEmployee);
    });

    const req = httpMock.expectOne(`${apiUrl}/employees/1`);
    expect(req.request.method).toEqual('GET');
    req.flush(dummyEmployee);
  });

  it('should create an employee via POST', () => {
    const newEmployee: Employee = { id: 0, name: 'Alice', position: 'Manager', hiringDate: '2023-09-14', salary: 70000 };

    service.createEmployee(newEmployee).subscribe((employee) => {
      expect(employee).toEqual(newEmployee);
    });

    const req = httpMock.expectOne(`${apiUrl}/employees`);
    expect(req.request.method).toEqual('POST');
    req.flush(newEmployee);
  });

  it('should delete an employee by id via DELETE', () => {
    const employeeIdToDelete = 1;

    service.deleteEmployee(employeeIdToDelete).subscribe(() => {
      expect().nothing();
    });

    const req = httpMock.expectOne(`${apiUrl}/employees/${employeeIdToDelete}`);
    expect(req.request.method).toEqual('DELETE');
    req.flush(null); // Simulate a successful DELETE request
  });

});
