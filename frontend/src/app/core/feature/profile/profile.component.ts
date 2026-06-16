import { Component, inject } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService } from '../auth/services/auth.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, of } from 'rxjs';
import { StatCardsComponent } from "../../../shared/stat-cards/stat-cards.component";
import { MatAnchor } from "@angular/material/button";
import { UserService } from '../auth/services/user.service';

@Component({
  selector: 'app-profile',
  imports: [MatIcon, TranslocoModule, StatCardsComponent, MatAnchor],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export default class ProfileComponent {
	
	private authService = inject(AuthService);
	
	public user = toSignal(
		this.authService.me().pipe(
			map(response => response.data)
		)
	);
}
