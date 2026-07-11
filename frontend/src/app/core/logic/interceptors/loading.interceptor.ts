import { HttpInterceptorFn } from '@angular/common/http';
import { LoadingService } from '../services/loading.service';
import { inject } from '@angular/core';
import { delay, finalize, of, switchMap, timeout } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  const threshold = 500;
  let isSpinnerShowing = false;

  //activates the spinner after 500ms
  const timeoutId = setTimeout(() => {
    loadingService.show();
    isSpinnerShowing = true;
  }, threshold);

  return next(req).pipe(finalize(() => {
    //after the request ends
    clearTimeout(timeoutId);

    if (isSpinnerShowing) {
      loadingService.hide();
    }
  }))
};

