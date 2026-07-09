import { inject } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { CanActivateFn, Router } from '@angular/router';
import { filter, map, take } from 'rxjs';
import { SessionService } from '../services/session.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(SessionService);
  const router = inject(Router);

  return toObservable(auth.loading).pipe(
    filter((loading) => !loading),
    take(1),
    map(() => (auth.isAuthenticated() ? true : router.createUrlTree(['/']))),
  );
};
