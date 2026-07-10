import { Component, inject } from '@angular/core';
import { map, merge, startWith } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterLink } from '@angular/router';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
	selector: 'app-landing-page',
	standalone: true,
	imports: [NavbarComponent, CommonModule, RouterLink, TranslocoModule],
	templateUrl: './landing-page.component.html',
	styleUrl: './landing-page.component.scss',
})
export class LandingPageComponent {
	//instead of using the constructor, we can inject it
	private readonly dialog = inject(MatDialog);

	//isDialogOpen$ is an observable stream that will emit values over time
	//merge merges two observables into one stream (whichever one emits a value, it gets passed down the chain)
	isDialogOpen$ = merge(
		this.dialog.afterOpened.pipe(map(() => true)), //pipe is used to manipulate data
		this.dialog.afterAllClosed.pipe(map(() => false)) //map transforms data emitted by a stream
	).pipe(
		startWith(false)
	);
}
