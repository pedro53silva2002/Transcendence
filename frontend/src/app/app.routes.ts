import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: 'profile',
		loadComponent: () => import('./core/profile/profile.component')
	  },
];
