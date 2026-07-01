import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorService } from '../services/error.service';
import { SessionService } from '../services/session.service';
import { Router } from '@angular/router';

/**
 * HTTP interceptor that automatically converts all failed HTTP calls
 * into ApiError instances before they reach your services or components.
 *
 * WHY AN INTERCEPTOR?
 * Without this, every service would need its own try/catch to convert
 * HttpErrorResponse → ApiError. The interceptor does it once, globally,
 * so services always receive a consistent ApiError when a call fails.
 *
 * HOW IT WORKS:
 * Angular's HttpClient returns Observables. The interceptor sits in the
 * pipeline between the HTTP call and the caller. When an error occurs,
 * catchError intercepts it before it reaches your .subscribe() or await.
 *
 * FUNCTIONAL vs CLASS INTERCEPTORS:
 * We use the modern functional style (HttpInterceptorFn) introduced in
 * Angular 15+. It avoids the boilerplate of implementing a class and
 * works seamlessly with inject().
 *
 * REGISTER THIS in app.config.ts via:
 *   provideHttpClient(withInterceptors([errorInterceptor]))
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorService = inject(ErrorService);
  const sessionService = inject(SessionService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((err) => {

      const isLoginEndpoint = req.url.includes('/auth/login') || req.url.includes('/login');
      const isSessionCheck = req.url.includes('/auth/me') || req.url.includes('/me');

      if ((err.status === 401 || err.status === 403) && !isLoginEndpoint && !isSessionCheck) {
        sessionService.clearSession();
        router.navigate(['/']);
      }

      const apiError = errorService.handle(err);
      return throwError(() => apiError);
    }),
  );
};
