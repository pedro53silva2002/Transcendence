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
import { AuthService as ApiAuthService } from '../../feature/auth/services/auth.service';
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { CloseButtonComponent } from '../../../shared/components/close-button/close-button.component';
import { GoogleAuthButtonComponent } from '../../auth/google-auth-button/google-auth-button.component';
import { UserService } from '../../feature/auth/services/user.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../logic/services/session.service';

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
  private readonly apiAuthService = inject(ApiAuthService);
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
  private readonly submitTrigger$ = new Subject<void>();

  constructor() {
    this.form.controls.username.statusChanges
      .pipe(takeUntilDestroyed(this.destroyRef)) // ← prevents memory leak
      .subscribe(() => this.cdr.detectChanges());
    this.form.controls.email.statusChanges
      .pipe(takeUntilDestroyed(this.destroyRef)) // ← prevents memory leak
      .subscribe(() => this.cdr.detectChanges());
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
            catchError(() => EMPTY),
            finalize(() => this.submitting.set(false)),
          ),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe();
  }

  private readonly validateUsername = (
    control: AbstractControl,
  ): Observable<ValidationErrors | null> => {
    const value = (control.value ?? '').trim();
    if (!value) return of(null);
    return timer(400).pipe(
      switchMap(() =>
        from(
          this.userService.search({
            search: { username: { op: 'EQUAL', value } },
            pageSize: 1,
          }),
        ),
      ),
      map((res) => ((res.data?.content.length ?? 0) > 0 ? { usernameTaken: true } : null)),
      catchError(() => of(null)),
    );
  };

  private readonly validateUniqueEmail = (
    control: AbstractControl,
  ): Observable<ValidationErrors | null> => {
    const value = (control.value ?? '').trim();
    if (!value) return of(null);
    return timer(400).pipe(
      switchMap(() =>
        from(
          this.userService.search({
            search: { email: { op: 'EQUAL', value } },
            pageSize: 1,
          }),
        ),
      ),
      map((res) => ((res.data?.content.length ?? 0) > 0 ? { emailTaken: true } : null)),
      catchError(() => of(null)),
    );
  };

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

  readonly form = new FormGroup(
    {
      username: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required],
        asyncValidators: [this.validateUsername],
        updateOn: 'blur',
      }),
      email: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required, Validators.email, this.validateUserEmail],
        asyncValidators: [this.validateUniqueEmail],
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
}
