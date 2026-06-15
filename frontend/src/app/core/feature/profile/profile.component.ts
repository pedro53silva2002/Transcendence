import { Component, inject } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService } from '../auth/services/auth.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, of } from 'rxjs';
import { StatCardsComponent } from "../../../shared/stat-cards/stat-cards.component";
import { MatAnchor } from "@angular/material/button";

@Component({
  selector: 'app-profile',
  imports: [MatIcon, TranslocoModule, StatCardsComponent, MatAnchor],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export default class ProfileComponent {
	
	private authService = inject(AuthService);

	private mockApiResponse = {
		data : {
			displayName: 'Cristiano Ronaldo',
			username: 'cr7',
			profilePhotoUrl: 'images/globe.png'
			// profilePhotoUrl: null
		}
	};
	
	public user = toSignal(
		// this.authService.me().pipe(
		of(this.mockApiResponse).pipe(
			map(response => response.data)
		)
	);
}
