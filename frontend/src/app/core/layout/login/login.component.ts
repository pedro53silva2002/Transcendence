import { ChangeDetectionStrategy, Component, OnInit, OnDestroy, ViewEncapsulation, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { catchError, exhaustMap, finalize, firstValueFrom, from, of, Subject, takeUntil, tap } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { SessionService } from '../../logic/services/session.service';
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { CloseButtonComponent } from '../../../shared/components/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';
import { MatIcon } from "@angular/material/icon";

@Component({
	selector: 'app-login',
	imports: [
		MatDialogModule,
		MatFormFieldModule,
		MatInputModule,
		MatButtonModule,
		ReactiveFormsModule,
		GoogleAuthButtonComponent,
		CloseButtonComponent,
		TranslocoModule,
		MatIcon,
	],
	templateUrl: './login.component.html',
	styleUrl: './login.component.scss',
	encapsulation: ViewEncapsulation.None,
	//the html will only be redesigned if a signal changes or if an html event is set
	changeDetection: ChangeDetectionStrategy.OnPush, 
})
export class LoginComponent implements OnInit, OnDestroy {
	private readonly apiAuthService = inject(ApiAuthService);
	private readonly authService = inject(SessionService);
	private readonly tokenStorage = inject(TokenStorageService);
	private readonly router = inject(Router);
	private readonly dialogRef = inject(MatDialogRef<LoginComponent>);

	readonly form = new FormGroup({
		email: new FormControl('', {
			nonNullable: true,
			validators: [Validators.required, Validators.email],
		}),
		password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
	});

	showOAuthErrorMessage = false;
	showLoginErrorMessage = signal<boolean>(false);
	protected readonly passwordVisible = signal(false);
	public isLoading = signal<boolean>(false);

	private readonly submit$ = new Subject<void>();
	private readonly destroy$ = new Subject<void>();

	ngOnInit(): void {

		if (sessionStorage.getItem('auth_origin') === 'login') {
			if (this.authService.getOAuthResult() === false) {
				this.showOAuthErrorMessage = true;
			} else {
				this.showOAuthErrorMessage = false;
			}
		}

		this.submit$.pipe(
			exhaustMap(() => {
				// if (this.form.invalid) {
				// 	this.form.markAllAsTouched();
				// 	return of(null);
				// }

				this.isLoading.set(true);
				this.showLoginErrorMessage.set(false);

				const { email, password } = this.form.getRawValue();

				return from(this.apiAuthService.login({ email, password })).pipe(
					tap(async (result) => {
						if (result.data) {
							this.tokenStorage.saveAccessToken(result.data.token);
							this.tokenStorage.saveRefreshToken(result.data.refreshToken);

							const ok = await firstValueFrom(this.authService.loadMe());
							if (ok) {
								this.dialogRef.close();
								this.router.navigate(['/home']);
							}
						}
					}),
					catchError((error) => {
						console.log('entrou 3');
						this.showLoginErrorMessage.set(true);
						return of(null);
					}),
					finalize(() => this.isLoading.set(false))
				);
			}),
			takeUntil(this.destroy$)
		).subscribe();

		sessionStorage.setItem('auth_origin', 'login');
	}

	submit(): void {
		this.submit$.next();
	}

	ngOnDestroy(): void {
		this.destroy$.next();
		this.destroy$.complete();
	}
}
