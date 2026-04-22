import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationPopUp } from './shared/pop-up/confirmation-pop-up/confirmation-pop-up';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatButtonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');

  constructor(private dialog: MatDialog) {}
  
  openPopUp(): void {
	const dialogRef = this.dialog.open(ConfirmationPopUp, {
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


