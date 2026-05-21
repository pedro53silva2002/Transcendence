import { Component, inject, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Subscription } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService } from '../../feature/auth/services/AuthService';

@Component({
  selector: 'app-google-auth-button',
  imports: [MatButtonModule, TranslocoModule],
  templateUrl: './google-auth-button.component.html',
  styleUrl: './google-auth-button.component.scss',
})
export class GoogleAuthButtonComponent implements OnDestroy {
  private readonly authService = inject(AuthService);
  redirectSubscription?: Subscription;

  redirectToGoogle() {
    this.redirectSubscription = this.authService.getGoogleRedirectUrl().subscribe({
      //if the subscribe succeeds
      next: (res: { data?: { authorizationUrl: string } }) => {
        window.location.href = res.data?.authorizationUrl ?? '/';
      },
      error: (err: unknown) => console.error(err),
    });
  }

  ngOnDestroy() {
    this.redirectSubscription?.unsubscribe();
  }
}
