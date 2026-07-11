import { Component, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet,
		LoadingSpinnerComponent
	],
	encapsulation: ViewEncapsulation.None, //permite alterar o css de componentes do angular materials sem usar ng-deep
	templateUrl: './app.html',
	styleUrl: './app.scss'
})
export class App {
}


