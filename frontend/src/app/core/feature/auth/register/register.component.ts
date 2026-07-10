import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DestroyRef,
  OnInit,
  ViewEncapsulation,
  computed,
  inject,
  signal,
} from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import {
  catchError,
  delay,
  EMPTY,
  exhaustMap,
  finalize,
  from,
  map,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
  timer,
} from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CloseButtonComponent } from '../../../../shared/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../google-auth-button/google-auth-button.component';
import { SessionService } from '../../../logic/services/session.service';
import { UserService } from '../../../logic/services/user.service';
import { TokenStorageService } from '../../../logic/services/token-storage.service';
import { AuthService } from '../../../logic/services/auth.service';
import { ApiError } from '../../../logic/model/api-error.model';

@Component({
  standalone: true,
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
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent implements OnInit {
  private readonly apiAuthService = inject(AuthService);
  private readonly sessionService = inject(SessionService);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);
  private readonly dialogRef = inject(MatDialogRef<RegisterComponent>);
  private readonly userService = inject(UserService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);
  protected readonly passwordVisible = signal(false);
  protected readonly confirmPasswordVisible = signal(false);
  protected readonly submitting = signal(false);
  protected readonly passwordValue = signal('');
  protected readonly emailValue = signal('');
  private readonly submitTrigger$ = new Subject<void>();

  constructor() {
    this.form.controls.username.statusChanges
      .pipe(takeUntilDestroyed(this.destroyRef)) // ← prevents memory leak
      .subscribe(() => this.cdr.detectChanges());
    this.form.controls.email.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef)) // ← prevents memory leak
      .subscribe((value) => this.emailValue.set(value));
    this.form.controls.password.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((value) => this.passwordValue.set(value));
  }

  showOAuthErrorMessage = false;

  ngOnInit(): void {
    if (sessionStorage.getItem('auth_origin') === 'register') {
      if (this.sessionService.getOAuthResult() === false) {
        this.showOAuthErrorMessage = true;
      } else {
        this.showOAuthErrorMessage = false;
      }
    }

    sessionStorage.setItem('auth_origin', 'register');

    this.submitTrigger$
      .pipe(
        exhaustMap(() =>
          from(this.apiAuthService.register(this.form.getRawValue())).pipe(
            switchMap((result) => {
              if (result.data) {
                this.tokenStorage.saveAccessToken(result.data.token);
                this.tokenStorage.saveRefreshToken(result.data.refreshToken);
                return this.sessionService.loadMe();
              }
              return of(false);
            }),
            tap((ok) => {
              this.dialogRef.close();
              this.router.navigate([ok ? '/home' : '/login']);
            }),
            catchError((err) => {
              this.applyRegisterError(err);
              return EMPTY;
            }),
            finalize(() => this.submitting.set(false)),
          ),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  validateUserEmail: ValidatorFn = (control) => {
    const email = control.value;
    if (!email) return null;

    if (!/[.]/.test(email)) return { dot: true };
    return null;
  };

  passwordMatches: ValidatorFn = (group) => {
    const pwd = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    if (!pwd || !confirm) return null;
    return pwd === confirm ? null : { passwordMismatch: true };
  };

  passwordRulesValidator: ValidatorFn = (control) => {
    const v = control.value ?? '';
    const errors: ValidationErrors = {};
    if (v.length < 8 || v.length > 20) errors['length'] = true;
    if (!/[a-z]/.test(v)) errors['lowercase'] = true;
    if (!/[A-Z]/.test(v)) errors['uppercase'] = true;
    if (!/[0-9]/.test(v)) errors['digits'] = true;
    if (!/[^A-Za-z0-9]/.test(v)) errors['special'] = true;
    return Object.keys(errors).length ? errors : null;
  };

  protected readonly passwordRules = computed(() => {
    const v = this.passwordValue();
    return {
      length: v.length >= 8 && v.length <= 20,
      lowercase: /[a-z]/.test(v),
      uppercase: /[A-Z]/.test(v),
      digits: /[0-9]/.test(v),
      special: /[^A-Za-z0-9]/.test(v),
    };
  });

  protected readonly emailValidator = computed(() => {
    const v = this.emailValue();
    if (!v) return undefined;
    return {
      emailRegex: /^[^@\s]+@[^@\s.]+(?:\.[^@\s.]+)+$/.test(v),
    };
  });

  readonly form = new FormGroup(
    {
      username: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required],
        updateOn: 'blur',
      }),
      email: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required, Validators.email, this.validateUserEmail],
        updateOn: 'blur',
      }),
      password: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required, this.passwordRulesValidator],
      }),
      confirmPassword: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required],
      }),
    },
    { validators: this.passwordMatches },
  );

  submit(): void {
    if (this.form.invalid) return;
    this.showOAuthErrorMessage = false;
    this.submitting.set(true);
    this.submitTrigger$.next();
  }

  /**
   * Surfaces a failed register call on the relevant form field.
   *
   * The async validators (validateUsername / validateUniqueEmail) run on blur,
   * so a fast submit — or another user registering the same value in the
   * meantime — can reach the backend, which replies with a 409 Conflict whose
   * message names the offending field. We map that onto the same
   * emailTaken/usernameTaken errors the template already renders.
   */
  private applyRegisterError(err: unknown): void {
    if (!(err instanceof ApiError) || err.statusCode !== 409) return;

    const message = (err.message ?? '').toLowerCase();
    const control = message.includes('email')
      ? this.form.controls.email
      : this.form.controls.username;
    const errorKey = control === this.form.controls.email ? 'emailTaken' : 'usernameTaken';

    control.setErrors({ ...control.errors, [errorKey]: true });
    control.markAsTouched();
    this.cdr.detectChanges();
  }
}
