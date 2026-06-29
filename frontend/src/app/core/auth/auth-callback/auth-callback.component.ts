import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SessionService } from '../../logic/services/session.service';
import { TokenStorageService } from '../../logic/services/token-storage.service';
import { LoadingSpinnerComponent } from '../../../shared/loading-spinner/loading-spinner.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-auth-callback',
  imports: [LoadingSpinnerComponent],
  templateUrl: './auth-callback.component.html',
  styleUrl: './auth-callback.component.scss',
})
export class AuthCallbackComponent implements OnInit {
  private readonly destroyRef = inject(DestroyRef);
  private readonly tokenStorage = inject(TokenStorageService);
  private authService = inject(SessionService);

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) {}

  ngOnInit() {
    const error = this.route.snapshot.queryParamMap.get('error');
    if (error) {
      this.authService.setOAuthSuccess(false);
      this.router.navigate(['/']);
      return;
    }

    // tokens are passed in the URL fragment (#token=...&refreshToken=...) by the backend
    // so they never appear in server logs or referer headers
    const fragment = new URLSearchParams(window.location.hash.slice(1));
    const token = fragment.get('token');
    const refreshToken = fragment.get('refreshToken');

    if (!token || !refreshToken) {
      this.router.navigate(['/'], { queryParams: { authError: 'missing_tokens' } });
      return;
    }

    this.tokenStorage.saveAccessToken(token);
    this.tokenStorage.saveRefreshToken(refreshToken);

    //o takeUntilDestroyed limpa a subscrição assim que o componente sai do ecrã (http requests não precisam disto, mas é boa prática fazer)
    this.authService
      .loadMe()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((success) => {
        if (success) {
          this.authService.setOAuthSuccess(true);
          this.router.navigate(['/home']);
        } else {
          this.authService.setOAuthSuccess(false);
          this.router.navigate(['/']);
        }
      });
  }
}
