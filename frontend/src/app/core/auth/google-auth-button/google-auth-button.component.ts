import { Component, inject, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Subscription } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService } from '../../feature/auth/services/auth.service';
import { SessionService } from '../../logic/services/session.service';

@Component({
  selector: 'app-google-auth-button',
  imports: [MatButtonModule, TranslocoModule],
  templateUrl: './google-auth-button.component.html',
  styleUrl: './google-auth-button.component.scss',
})
export class GoogleAuthButtonComponent implements OnDestroy {
  private readonly authService = inject(AuthService);
  redirectSubscription?: Subscription;

  constructor(private sessionService: SessionService) {}

  redirectToGoogle() {
    // Clear any stale session so the OAuth callback can save fresh tokens
    // without the app initializer firing the old expired token at /auth/me.
    this.sessionService.clearSession();
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
