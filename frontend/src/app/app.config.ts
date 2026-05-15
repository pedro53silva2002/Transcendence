import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { errorInterceptor } from './core/logic/interceptors/error.interceptor';
import { jwtInterceptor } from './core/logic/interceptors/jwt.interceptor';
import { AuthService } from './core/logic/services/auth.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([jwtInterceptor, errorInterceptor])),

    provideAppInitializer(() => {
      const auth = inject(AuthService);
      return auth.loadMe();
    }),
  ],
};
