import { CanActivateFn, Router } from '@angular/router';
import { SessionService } from '../../../logic/services/session.service';
import { inject } from '@angular/core';

export const tripMemberGuard: CanActivateFn = (route, state) => {

  const authService = inject(SessionService);
  const router = inject(Router);

  const tripId = Number(route.paramMap.get('id'));
  const user = authService.me();

  if (!tripId || !user) {
    router.navigate(['/home']);
    return false;
  }

  const isMember = user.trips?.some(trip => trip.tripId === tripId);

  if (isMember) {
    return true;
  }

  router.navigate(['/home']);

  return false;
};
