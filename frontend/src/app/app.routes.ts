import { Routes } from '@angular/router';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';
import { LandingPageComponent } from './core/layout/landing-page/landing-page.component';
import { authGuard } from './core/logic/services/authGuard.service';
import { guestGuard } from './core/logic/services/guestGuard.service';

export const routes: Routes = [
  { path: '', component: LandingPageComponent, canActivate: [guestGuard] },
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
  { path: '**', redirectTo: '' },
];
