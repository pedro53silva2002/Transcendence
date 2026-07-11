import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorService } from '../services/error.service';

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

  return next(req).pipe(
    catchError((err) => {
      // NOTE: 401s are owned by jwtInterceptor, which attempts a token refresh
      // and only clears the session if that refresh fails. Handling 401 here too
      // would tear down the session before the refresh gets a chance to run.
      //
      // 403s are NOT a session problem — they mean the (validly authenticated)
      // user isn't allowed to perform this specific action (e.g. not a trip
      // admin). They must not clear the session or redirect; just surface the
      // error so the caller can show it.

      const apiError = errorService.handle(err);
      return throwError(() => apiError);
    }),
  );
};
