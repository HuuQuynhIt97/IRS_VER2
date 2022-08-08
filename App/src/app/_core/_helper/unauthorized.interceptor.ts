import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
} from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { AuthService } from '../_service/auth.service';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private router: Router) { }

    intercept(
        request: HttpRequest<unknown>,
        next: HttpHandler
    ): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            catchError((err) => {
                if (err.status === 401) {
                    this.authService.clearLocalStorage();
                    this.router.navigate(['login'], {
                        queryParams: { returnUrl: this.router.routerState.snapshot.url },
                    });
                    return throwError(err);
                }
                if (!environment.production) {
                    console.error(err);
                }
                return next.handle(err);
            })
        );
    }
}
