import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { GlobalErrorHandlerService } from './services/global-error-handler';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private errorHandler: GlobalErrorHandlerService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        return this.errorHandler.handleError(error);
      })
    );
  }
}
