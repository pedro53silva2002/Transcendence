import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-google-auth-button',
  imports: [MatButtonModule],
  templateUrl: './google-auth-button.component.html',
  styleUrl: './google-auth-button.component.scss',
})
export class GoogleAuthButtonComponent {
	redirectToGoogle() {
		window.location.href = (environment as any).apiUrl + '/auth/google';
	  }
}
