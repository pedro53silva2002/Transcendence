import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { AuthService } from '../../logic/services/auth.service';
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { CloseButtonComponent } from '../../../shared/components/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';

@Component({
	selector: 'app-register',
	imports: [
		MatDialogModule,
		MatIconModule,
		MatFormFieldModule,
		MatInputModule,
		TranslocoModule,
		MatButtonModule,
		ReactiveFormsModule,
		CloseButtonComponent,
		GoogleAuthButtonComponent,
	],
	templateUrl: './register.component.html',
	styleUrl: './register.component.scss',
	encapsulation: ViewEncapsulation.None,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent implements OnInit {
	private readonly apiAuthService = inject(ApiAuthService);
	private readonly authService = inject(AuthService);
	private readonly tokenStorage = inject(TokenStorageService);
	private readonly router = inject(Router);
	private readonly dialogRef = inject(MatDialogRef<RegisterComponent>);

	readonly form = new FormGroup({
		username: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
		email: new FormControl('', {
			nonNullable: true,
			validators: [Validators.required, Validators.email],
		}),
		password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
		confirmPassword: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
	});

	showOAuthErrorMessage = false;

	ngOnInit(): void {

		if (sessionStorage.getItem('auth_origin') === 'register') {
			if (this.authService.getOAuthResult() === false) {
				this.showOAuthErrorMessage = true;
			} else {
				this.showOAuthErrorMessage = false;
			}
		}

		sessionStorage.setItem('auth_origin', 'register');
	}

	async submit(): Promise<void> {
		this.showOAuthErrorMessage = false;
		if (this.form.invalid) return;
		const { username, email, password, confirmPassword } = this.form.getRawValue();
		if (password !== confirmPassword) {
			alert('password does not match');
			return;
		}
		try {
			const result = await this.apiAuthService.register({ username, email, password });
			if (result.data) {
				this.tokenStorage.saveAccessToken(result.data.token);
				this.tokenStorage.saveRefreshToken(result.data.refreshToken);
				const ok = await firstValueFrom(this.authService.loadMe());
				if (ok) {
					this.dialogRef.close();
					this.router.navigate(['/home']);
					return;
				}
			}
		} catch {
			// registration failed
		}
		this.dialogRef.close();
		this.router.navigate(['/login']);
	}
}
