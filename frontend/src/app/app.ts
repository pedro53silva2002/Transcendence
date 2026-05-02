import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmationPopUpComponent } from './shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { NavbarComponent } from './core/layout/navbar/navbar.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';
import { AppHomeComponent } from './core/layout/app-home/app-home.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, 
			MatButtonModule,
			MatDialogModule,
			MatSidenavModule,
			LoadingSpinnerComponent,
			ConfirmationPopUpComponent,
			AppHomeComponent
			],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');

  constructor(private dialog: MatDialog) {}
  
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
  }
}


