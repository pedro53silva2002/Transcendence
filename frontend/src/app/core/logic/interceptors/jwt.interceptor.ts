import {
  HttpBackend,
  HttpClient,
  HttpErrorResponse,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import {
  Observable,
  catchError,
  finalize,
  map,
  shareReplay,
  switchMap,
  tap,
  throwError,
} from 'rxjs';
<<<<<<< HEAD
import { AuthService } from '../services/auth.service';
=======
import { SessionService } from '../services/session.service';
>>>>>>> origin/feat/trips
import { TokenStorageService } from '../services/token-storage.service';
import { Router } from '@angular/router';
import { AuthResponseDto } from '../../feature/auth/dtos/auth.dto';
import { environment } from '../../../../environments/environment';

let refreshInProgress: Observable<string> | null = null;

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenStorage = inject(TokenStorageService);
<<<<<<< HEAD
  const authService = inject(AuthService);
=======
  const authService = inject(SessionService);
>>>>>>> origin/feat/trips
  const httpBackend = inject(HttpBackend);
  const router = inject(Router);

  const token = tokenStorage.getAccessToken();
  const authReq = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

  return next(authReq).pipe(
    catchError((error) => {
      if (!(error instanceof HttpErrorResponse && error.status === 401)) {
        return throwError(() => error);
      }
      const isExpired = error.headers.get('Token-Expired') === 'true';
      const refreshToken = tokenStorage.getRefreshToken();

      if (isExpired && refreshToken) {
        if (!refreshInProgress) {
          // HttpBackend bypasses all interceptors — no infinite loop.
          const http = new HttpClient(httpBackend);
          refreshInProgress = http
            .post<AuthResponseDto>(`${environment.apiUrl}/auth/refresh`, { refreshToken })
            .pipe(
              tap((res) => {
                tokenStorage.saveAccessToken(res.token);
                tokenStorage.saveRefreshToken(res.refreshToken);
              }),
              map((res) => res.token),
              shareReplay(1),
              finalize(() => {
                refreshInProgress = null;
              }),
            );
        }

        return refreshInProgress.pipe(
          switchMap((newToken) => {
            const retried = req.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` },
            });
            return next(retried);
          }),
          catchError(() => {
            authService.clearSession();
            router.navigate(['/']);
            return throwError(() => error);
          }),
        );
      }

      // Only force-logout if a token was actually sent but the server rejected it.
      // If there was no token, the user simply wasn't authenticated — don't redirect.
      if (token) {
        authService.clearSession();
        router.navigate(['/']);
      }
      return throwError(() => error);
    }),
  );
};
