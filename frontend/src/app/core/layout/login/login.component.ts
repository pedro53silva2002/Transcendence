import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
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

  readonly form = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email],
    }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  showOAuthErrorMessage = false;

  ngOnInit(): void {
    if (sessionStorage.getItem('auth_origin') === 'login') {
      if (this.authService.getOAuthResult() === false) {
        this.showOAuthErrorMessage = true;
      } else {
        this.showOAuthErrorMessage = false;
      }
    }

    //sets the auth_origin in the session storage to 'login'
    sessionStorage.setItem('auth_origin', 'login');
  }

  async submit(): Promise<void> {
    this.showOAuthErrorMessage = false;
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
}
