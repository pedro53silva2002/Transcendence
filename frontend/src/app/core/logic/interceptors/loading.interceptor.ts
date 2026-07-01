import { HttpInterceptorFn } from '@angular/common/http';
import { LoadingService } from '../services/loading.service';
import { inject } from '@angular/core';
import { delay, finalize, of, switchMap, timeout } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  const threshold = 0;
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

//testing version
// export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
//     const loadingService = inject(LoadingService);

//     // 🌟 FORÇA o incremento do contador mal o pedido entra no interceptor
//     loadingService.show();

//     // Bloqueia o envio na rede por 2.5 segundos para dar tempo de ver
//     return of(null).pipe(
//         delay(2500),
//         switchMap(() => next(req)),
//         finalize(() => {
//             // 🌟 FORÇA o decremento mal qualquer pedido termine
//             loadingService.hide();
//         })
//     );
// };
