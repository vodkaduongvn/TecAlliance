import { Component, OnInit  } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EmployeeService } from '../services/employee.service';
import { NgbDateStruct, NgbCalendar, NgbDateAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { CustomDateAdapter, CustomDateParserFormatter } from '../datepicker/datepicker.adapter';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.css'],
  providers: [{provide: NgbDateAdapter, useClass: CustomDateAdapter},
    {provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter}]
})
export class EmployeeCreateComponent {
  employeeForm!: FormGroup;
  model!: NgbDateStruct;

  constructor(
    private fb: FormBuilder
    , private employeeService: EmployeeService
    , private calendar: NgbCalendar
    , private router:Router) {}

  ngOnInit(): void {
    this.employeeForm = this.fb.group({
      name: ['', [Validators.required]],
      position: [''],
      hiringDate: ['', [Validators.required, this.dateValidator]],
      salary: ['', [Validators.required, Validators.pattern('^[0-9]*$')]]
    });
  }

  dateValidator(control: FormControl) {
    const pattern = /^\d{4}-\d{2}-\d{2}$/;
    if (!pattern.test(control.value)) {
      return { invalidDate: true };
    }
    return null;
  }

  saveEmployee() {
    if (this.employeeForm?.valid) {
      console.log(this.employeeForm.value);
      this.employeeService.createEmployee(this.employeeForm.value).subscribe(newEmployee => {this.employeeService.getEmployees();});
    } else {
      console.log('Form contains errors.');
    }
  }

  goBack(){
    this.router.navigate(['/employees']);
  }
}
