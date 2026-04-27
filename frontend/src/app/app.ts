import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmationPopUpComponent } from './shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { SharedModule } from './shared/shared-module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatButtonModule, SharedModule, MatDialogModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
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


