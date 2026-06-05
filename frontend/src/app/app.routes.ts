import { Routes } from '@angular/router';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';
import { LandingPageComponent } from './core/layout/landing-page/landing-page.component';
import { authGuard } from './core/logic/services/authGuard.service';
import { guestGuard } from './core/logic/services/guestGuard.service';
import { AuthCallbackComponent } from './core/auth/auth-callback/auth-callback.component';

export const routes: Routes = [
  { path: '', component: LandingPageComponent, canActivate: [guestGuard] },

  //to verify if the google auth was successful and redirect to the home page
  { path: 'auth/callback', component: AuthCallbackComponent },

  {
    path: 'home',
    component: AppHomeComponent, //o componente que tem a nav-bar e router-outlet para navegar pelas páginas do site
    canActivate: [authGuard],
    children: [
      // { path: '', component: DashboardComponent }, //para a dashboard do user quando faz login
      {
        path: 'profile',
        loadComponent: () => import('./core/profile/profile.component'),
      },
    ],
  },
  {
    path: 'itinerary',
    loadComponent: () => import('./core/feature/itinerary/itinerary.component'),
    canActivate: [authGuard],
  },
  { path: '**', redirectTo: '' },
];
