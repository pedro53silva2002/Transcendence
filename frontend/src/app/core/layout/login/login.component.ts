import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  OnInit,
  ViewEncapsulation,
  inject,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { EMPTY, Subject, catchError, exhaustMap, finalize, of, switchMap, tap } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { SessionService } from '../../logic/services/session.service';
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { CloseButtonComponent } from '../../../shared/components/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';

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
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent implements OnInit {
  private readonly apiAuthService = inject(ApiAuthService);
  private readonly authService = inject(SessionService);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);
  private readonly dialogRef = inject(MatDialogRef<LoginComponent>);
  private readonly destroyRef = inject(DestroyRef);
  private readonly submitTrigger$ = new Subject<void>();

  protected readonly submitting = signal(false);

  readonly form = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  showOAuthErrorMessage = false;

  constructor() {
    this.submitTrigger$
      .pipe(
        exhaustMap(() =>
          this.apiAuthService.login(this.form.getRawValue()).pipe(
            switchMap((result) => {
              if (result.data) {
                this.tokenStorage.saveAccessToken(result.data.token);
                this.tokenStorage.saveRefreshToken(result.data.refreshToken);
                return this.authService.loadMe();
              }
              return of(false);
            }),
            tap((ok) => {
              if (ok) {
                this.dialogRef.close();
                this.router.navigate(['/home']);
              } else {
                alert('Invalid credentials');
              }
            }),
            catchError(() => {
              alert('Invalid credentials');
              return EMPTY;
            }),
            finalize(() => this.submitting.set(false)),
          ),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  ngOnInit(): void {
    if (sessionStorage.getItem('auth_origin') === 'login') {
      if (this.authService.getOAuthResult() === false) {
        this.showOAuthErrorMessage = true;
      } else {
        this.showOAuthErrorMessage = false;
      }
    }

    sessionStorage.setItem('auth_origin', 'login');
  }

  submit(): void {
    if (this.form.invalid) return;
    this.showOAuthErrorMessage = false;
    this.submitting.set(true);
    this.submitTrigger$.next();
  }
}
