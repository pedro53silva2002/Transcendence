import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  isDevMode,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { errorInterceptor } from './core/logic/interceptors/error.interceptor';
import { jwtInterceptor } from './core/logic/interceptors/jwt.interceptor';
import { SessionService } from './core/logic/services/session.service';
import { TranslocoHttpLoader } from './transloco-loader';
import { provideTransloco } from '@jsverse/transloco';
import { loadingInterceptor } from './core/logic/interceptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // Order matters: interceptors run top-down on the way out and the error
    // propagates back bottom-up. jwtInterceptor must be innermost so its 401
    // refresh handling runs BEFORE errorInterceptor can react to the 401.
    provideHttpClient(withInterceptors([loadingInterceptor, errorInterceptor, jwtInterceptor])),

    provideAppInitializer(() => {
      const auth = inject(SessionService);
      return auth.loadMe();
    }),
    provideTransloco({
      config: {
        availableLangs: ['en', 'pt', 'es'],
        defaultLang: 'en',
        // Remove this option if your application doesn't support changing language in runtime.
        reRenderOnLangChange: true,
        prodMode: !isDevMode(),
      },
      loader: TranslocoHttpLoader,
    }),
  ],
};
