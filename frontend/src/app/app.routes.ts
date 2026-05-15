import { Routes } from '@angular/router';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';
import { LandingPageComponent } from './core/layout/landing-page/landing-page.component';
import { AuthCallbackComponent } from './core/auth/auth-callback/auth-callback.component';

export const routes: Routes = [
	{ path: '', component: LandingPageComponent },
	
	//to verify if the google auth was successful and redirect to the home page
	{ path: 'auth/callback', component: AuthCallbackComponent },
	
	{
		path: 'home',
		component: AppHomeComponent, //o componente que tem a nav-bar e router-outlet para navegar pelas páginas do site
		children: [
			// { path: '', component: DashboardComponent }, //para a dashboard do user quando faz login
			{
				path: 'profile',
				loadComponent: () => import('./core/profile/profile.component')
			  },
		]
	},

];
