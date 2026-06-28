import { Routes } from '@angular/router';
import { UserDashboardComponent } from './core/layout/user-dashboard/user-dashboard.component';
import { LandingPageComponent } from './core/layout/landing-page/landing-page.component';
import { authGuard } from './core/logic/services/authGuard.service';
import { guestGuard } from './core/logic/services/guestGuard.service';
import { AuthCallbackComponent } from './core/auth/auth-callback/auth-callback.component';
import { AppLayoutComponent } from './core/layout/app-layout/app-layout.component';
import { PlanATripComponent } from './core/feature/plan-a-trip/plan-a-trip.component';
import { TripDashboardComponent } from './core/feature/trip-dashboard/trip-dashboard.component';
import { MyTripsComponent } from './core/feature/my-trips/my-trips.component';
import { tripMemberGuard } from './core/feature/trip-dashboard/guards/trip-member-guard';
import ProfileComponent from './core/feature/profile/profile.component';

export const routes: Routes = [
  {
    path: '',
    component: LandingPageComponent,
    canActivate: [guestGuard],
  },

  //to verify if the google auth was successful and redirect to the home page
  {
    path: 'auth/callback',
    component: AuthCallbackComponent,
  },

  //user dashboard (after logging in)
  {
    path: 'home',
    component: UserDashboardComponent, //o componente que tem a nav-bar e router-outlet para navegar pelas páginas do site
    canActivate: [authGuard],
  },

  //all other pages
  {
    path: '',
    component: AppLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
      	path: 'profile/:username',
      	component: ProfileComponent,
      },
      {
        path: 'plan-a-trip',
        component: PlanATripComponent,
      },
      {
        //route to edit the trip that will point to the same component
        path: 'plan-a-trip/edit/:id',
        component: PlanATripComponent,
      },
      {
        path: 'trip-dashboard/:id',
        component: TripDashboardComponent,
        canActivate: [tripMemberGuard],
      },
      {
        path: 'my-trips',
        component: MyTripsComponent,
      },
    ],
  },
];