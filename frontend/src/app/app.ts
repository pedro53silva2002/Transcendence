import { Component, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSidenavModule } from '@angular/material/sidenav';
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


