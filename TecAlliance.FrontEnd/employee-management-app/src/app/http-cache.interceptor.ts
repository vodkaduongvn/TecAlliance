import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpHandler,
  HttpEvent,
  HttpHeaders,
} from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export class HttpCacheInterceptor implements HttpInterceptor {
  private cache = new Map<string, HttpResponse<any>>();

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    const token = 'my_access_token';

    if (request.method !== 'GET') {
        if(request.method === 'POST'){
            this.cache.clear();
        }
      return next.handle(request);
    }

    if (token) {
        request = request.clone({
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          })
        });
      }

    const cachedResponse = this.cache.get(request.url);
    if (cachedResponse) {
      // Return a cached response as an observable
      return of(cachedResponse);
    }

    return next.handle(request).pipe(
      tap((event) => {
        if (event instanceof HttpResponse) {
          this.cache.set(request.url, event);
        }
      })
    );
  }
}
