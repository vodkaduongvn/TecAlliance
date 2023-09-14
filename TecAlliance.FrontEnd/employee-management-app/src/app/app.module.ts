import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule  } from '@angular/material/core';
import { HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { AgGridModule } from 'ag-grid-angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { EmployeeCreateComponent } from './employee-create/employee-create.component';
import { HttpCacheInterceptor } from './http-cache.interceptor';
import { GlobalErrorHandlerService } from './services/global-error-handler';
import { ErrorInterceptor } from './error-interceptor';


export const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'YYYY-MM-DD',
  },
  display: {
    dateInput: 'YYYY-MM-DD',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  },
};

@NgModule({
  declarations: [
    AppComponent,
    EmployeeListComponent,
    EmployeeDetailComponent,
    EmployeeCreateComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatInputModule,
    MatNativeDateModule,
    AgGridModule,
    NgbModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpCacheInterceptor,
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandlerService
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
