import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import type { MeDto, TripMembership } from '../dtos/me-dto.ts';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  apiURL = environment.apiUrl;
  private readonly http = inject(HttpClient);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);

  private readonly _loading = signal<boolean>(true);
  private readonly _me = signal<MeDto | undefined>(undefined);

  readonly loading = this._loading.asReadonly();
  readonly me = this._me.asReadonly();
  readonly isAuthenticated = computed(() => this._me() !== undefined);
  readonly user = computed(() => {
    const me = this._me();
    return me ? { username: me.username, displayName: me.displayName } : null;
  });

  clearSession(): void {
    this.tokenStorage.clearTokens();
    this._me.set(undefined);
    this._loading.set(false);
    this.router.navigate(['/']);
  }

  /**
   * Fetches the current user profile from the backend.
   *
   * Called once on app startup via APP_INITIALIZER (see app.config.ts).
   * Can be called again after login to refresh the session state.
   *
   * The backend reads the HttpOnly session cookie automatically thanks
   * to withCredentials: true — we never handle tokens manually.
   *
   * The finally block guarantees _loading is set to false
   * even if an error is thrown, so the app never gets stuck in a loading state.
   */
  //   async loadMe(): Promise<void> {
  //     this._loading.set(true);
  //     try {
  //       const me = await firstValueFrom(
  //         this.http.get<MeDto>(`${(environment as any).apiUrl}/auth/me`, { withCredentials: true }),
  //       );
  //       localStorage.setItem('avatar', me.profilePicture ?? '');
  //       this._me.set({
  //         ...me,
  //         trips: me.trips ?? [],
  //       });
  //     } catch {
  //       this._me.set(undefined);
  //     } finally {
  //       this._loading.set(false);
  //     }
  //   }

  loadMe(): Observable<boolean> {
    return this.http.get<MeDto>(`${this.apiURL}/auth/me`).pipe(
      //pipe serve para executar algo sobre o observable antes de ser subscrito
      tap((user) => this._me.set(user)), //tap é usado para efetuar tarefas secundárias sem alterar o fluxo principal do método
      map(() => true), //o map recebe do tap, mas neste caso ignora o valor do tap e simplesmente retorna true
      catchError(() => {
        this._me.set(undefined);
        return of(false); //para retornar false num observable, o of serve para criá-lo com false
      }),
    );
  }

  // ── Permission helpers ────────────────────────────────────────────────────

  canAccessTrip(trip: TripMembership): boolean {
    return this._me()?.trips?.includes(trip) ?? false;
  }

  /**
   *
   * @returns returns the Google URL for the user to be able to login with the Google account
   */
  getGoogleRedirectUrl() {
    const url = this.apiURL + `/auth/google/url`;
    return this.http.get<{ authorizationUrl: string }>(url);
  }
}
