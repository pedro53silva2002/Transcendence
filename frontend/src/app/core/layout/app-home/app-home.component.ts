import { Component, inject, OnInit, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCardComponent } from "../../../shared/dashboard-card/dashboard-card.component";
import { NavbarComponent } from '../navbar/navbar.component';
import { AuthService } from '../../logic/services/auth.service';
import { MeDto } from '../../feature/auth/AuthDtos';

@Component({
	selector: 'app-app-home',
	imports: [TranslocoModule,
		MatButtonModule,
		DashboardCardComponent,
		NavbarComponent],
	templateUrl: './app-home.component.html',
	styleUrl: './app-home.component.scss',
})
export class AppHomeComponent implements OnInit {
	private readonly authService = inject(AuthService);
	public user = signal<MeDto | null>(null);

	ngOnInit(): void {
		try {
			const response = this.authService.me();
			if (response) {
				this.user.set(response);
			}
		} catch (error) {
			console.error('Erro ao carregar utilizador:', error);
			this.user.set(null);
		}
	}
}
