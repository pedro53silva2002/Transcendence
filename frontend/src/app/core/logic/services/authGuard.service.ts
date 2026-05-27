import { inject } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { CanActivateFn, Router } from '@angular/router';
import { filter, map, take } from 'rxjs';
<<<<<<< HEAD
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
=======
import { SessionService } from './session.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(SessionService);
>>>>>>> origin/feat/trips
  const router = inject(Router);

  return toObservable(auth.loading).pipe(
    filter((loading) => !loading),
    take(1),
    map(() => (auth.isAuthenticated() ? true : router.createUrlTree(['/']))),
  );
};
