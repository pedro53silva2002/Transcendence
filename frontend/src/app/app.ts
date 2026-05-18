import { Component, Input, signal, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmationPopUpComponent } from './shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { NavbarComponent } from './core/layout/navbar/navbar.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';
import { InputLabelComponent } from './shared/components/input-label/input-label.component';
import { FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet,
		MatButtonModule,
		MatDialogModule,
		MatSidenavModule,
		MatFormFieldModule,
		MatInputModule
	],
	encapsulation: ViewEncapsulation.None, //permite alterar o css de componentes do angular materials sem usar ng-deep
	templateUrl: './app.html',
	styleUrl: './app.scss'
})
export class App {

	// popup
	/*protected readonly title = signal('frontend');
	controlo = new FormControl('', [Validators.required, Validators.email]);
	constructor(private dialog: MatDialog) { }

	openPopUp(): void {
		const dialogRef = this.dialog.open(ConfirmationPopUpComponent, {
			data: {
				title: "ola",
				message: "coucou"
			}
		});

		dialogRef.afterClosed().subscribe(response => {
			if (response === true)
				console.log('ola');
			else
				console.log('adeus');
		});
	}*/
}


