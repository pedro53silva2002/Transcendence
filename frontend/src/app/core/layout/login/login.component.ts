<<<<<<< HEAD
import { ChangeDetectionStrategy, Component, ViewEncapsulation, inject } from '@angular/core';
=======
import { ChangeDetectionStrategy, Component, OnInit, OnDestroy, ViewEncapsulation, inject, signal } from '@angular/core';
>>>>>>> origin/feat/trips
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
<<<<<<< HEAD
import { firstValueFrom } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { AuthService } from '../../logic/services/auth.service';
=======
import { catchError, exhaustMap, firstValueFrom, from, of, Subject, takeUntil, tap } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { SessionService } from '../../logic/services/session.service';
>>>>>>> origin/feat/trips
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { CloseButtonComponent } from '../../../shared/components/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';
import { MatIcon } from "@angular/material/icon";

@Component({
<<<<<<< HEAD
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
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly apiAuthService = inject(ApiAuthService);
  private readonly authService = inject(AuthService);
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

  async submit(): Promise<void> {
    if (this.form.invalid) return;
    const { email, password } = this.form.getRawValue();
    try {
      const result = await this.apiAuthService.login({ email, password });
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
      // login failed
    }
    alert('Invalid credentials');
  }
=======
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
		username: new FormControl('', {
			nonNullable: true,
			validators: [Validators.required],
		}),
		password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
	});

	showOAuthErrorMessage = false;
	showLoginErrorMessage = signal<boolean>(false);
	protected readonly passwordVisible = signal(false);

	//we use subjects because we cannot use RxJS operators directly in HTML events
	//this creates an open, empty pipeline waiting for data (it listens and we also can inject data)
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
				this.showLoginErrorMessage.set(false);

				const { username, password } = this.form.getRawValue();

				//form operator transforms the promise service result into an observable, so we can use pipe
				return from(this.apiAuthService.login({ username, password })).pipe(
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
						this.showLoginErrorMessage.set(true);
						return of(null);
					})
				);
			}),
			takeUntil(this.destroy$)
		).subscribe();

		sessionStorage.setItem('auth_origin', 'login');
	}

	submit(): void {
		this.submit$.next(); //activates the exhaustMap
	}

	ngOnDestroy(): void {
		this.destroy$.next(); //tells the takeUntil to unsubscribe
		this.destroy$.complete(); //closes the destroy channel
	}
>>>>>>> origin/feat/trips
}
