import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../logic/services/auth.service';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
	selector: 'app-auth-callback',
	imports: [LoadingSpinnerComponent],
	templateUrl: './auth-callback.component.html',
	styleUrl: './auth-callback.component.scss',
})
export class AuthCallbackComponent implements OnInit {

	private readonly destroyRef = inject(DestroyRef);

	constructor(
		private route: ActivatedRoute,
		private authService: AuthService,
		private router: Router
	) { }

	ngOnInit() {
		const error = this.route.snapshot.queryParamMap.get('error');
		if (error) {
			this.router.navigate(['/'], { queryParams: { authError: error } });
			return;
		}

		//o takeUntilDestroyed limpa a subscrição assim que o componente sai do ecrã (http requests não precisam disto, mas é boa prática fazer)
		this.authService.loadMe().pipe(takeUntilDestroyed(this.destroyRef)).subscribe(success => {
			if (success) {
				this.router.navigate(['/home']);
			} else {
				this.router.navigate(['/login'], { queryParams: { error: 'session_failed' } });
			}
		});

	}
}
