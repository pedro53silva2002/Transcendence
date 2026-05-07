import { Routes } from '@angular/router';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';

export const routes: Routes = [
	{
		path: '',
		component: AppHomeComponent,
		children: [
			{
				path: 'profile',
				loadComponent: () => import('./core/profile/profile.component')
			  },
		]
	},
	// {
	// 	path: 'login',
	// 	loadComponent: () => import('.core/layout/login.component')
	// }
];
