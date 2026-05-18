import { Component, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { AuthService } from '../../logic/services/auth.service';

@Component({
	selector: 'app-google-auth-button',
	imports: [MatButtonModule],
	templateUrl: './google-auth-button.component.html',
	styleUrl: './google-auth-button.component.scss',
})
export class GoogleAuthButtonComponent implements OnDestroy {
	redirectSubscription?: Subscription;

	constructor(private http: HttpClient, private authService: AuthService) { }

	redirectToGoogle() {
		this.redirectSubscription = this.authService.getGoogleRedirectUrl()
			.subscribe({
				//if the subscribe succeeds
				next: (res) => {
					window.location.href = res.authorizationUrl;
				},
				error: (err) => console.error(err)
			});
	}

	ngOnDestroy() {
		this.redirectSubscription?.unsubscribe();
	}
}
