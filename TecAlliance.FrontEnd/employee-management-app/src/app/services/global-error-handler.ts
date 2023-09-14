import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandlerService {
  handleError(error: any) {
    if (error instanceof HttpErrorResponse) {
      console.error('HTTP Error:', error);
    } else {
      console.error('An error occurred:', error);
    }
    return throwError(error);
  }
}
