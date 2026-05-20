import { ChangeDetectionStrategy, Component, ViewEncapsulation, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService as ApiAuthService } from '../../feature/auth/AuthService';
import { AuthService } from '../../logic/services/auth.service';
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
