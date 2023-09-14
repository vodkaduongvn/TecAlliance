import { Component, OnInit } from '@angular/core';
import { Employee } from '../models/employee.model';
import { EmployeeService } from '../services/employee.service';
import { ColDef } from 'ag-grid-community';
import { Router } from '@angular/router';
import { GridOptions } from 'ag-grid-community';


@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
 gridOptions!: GridOptions;
  columnDefs : ColDef[] =  [
    { headerName: 'ID', field: 'id' },
    { headerName: 'Name', field: 'name' },
    { headerName: 'Position', field: 'position' },
    { headerName: 'Hiring Date', field: 'hiringDate' },
    { headerName: 'Salary', field: 'salary' },
    {
      headerName: 'Action',
      cellRenderer: this.viewDetailsRenderer,
      cellRendererParams: {
        onClick: this.onViewDetails.bind(this),
      },
    }
  ];

  rowData: Employee[] = [];

  constructor(private employeeService: EmployeeService, private router: Router) {}

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe(data => {
      this.rowData = data;
    });
  }

  onViewDetails(id:any) {
    // Navigate to the employee detail page with the employee ID
    this.router.navigate(['/employees', id]);
  }

  viewDetailsRenderer(params:any) {
    const button = document.createElement('button');
    button.innerHTML = 'View Details';
    button.addEventListener('click', function () {
      params.onClick(params.data.id); // Pass the row ID to the click handler
    });
    return button;
  }
  
  create(){
    this.router.navigate(['/create']);
  }
}
