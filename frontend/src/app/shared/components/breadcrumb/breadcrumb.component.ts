import { Component, inject, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Location } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [MatIconModule,
			MatButtonModule,
			RouterModule
  			],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss',
})
export class BreadcrumbComponent {
	private location = inject(Location);
	private router = inject(Router);
	@Input() previousPage: string = '';
	@Input() backRoute: string = '/';
}
