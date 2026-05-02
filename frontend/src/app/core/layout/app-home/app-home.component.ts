import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { BreadcrumbComponent } from '../../../shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-app-home',
  imports: [NavbarComponent,
			RouterOutlet,
			BreadcrumbComponent
 			],
  templateUrl: './app-home.component.html',
  styleUrl: './app-home.component.scss',
})
export class AppHomeComponent {}
