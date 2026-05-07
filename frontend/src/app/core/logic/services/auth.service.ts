import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../environments/environment';
import type { MeDto, TripMembership } from '../dtos/me-dto.ts';

/**
 * Manages the authenticated user's session state for the entire application.
 *
 * WHY SIGNALS?
 * Angular Signals are the modern lightweight way to manage reactive state.
 * Compared to BehaviorSubject:
 *   - No need to unsubscribe (signals are not Observables)
 *   - computed() is cleaner than combineLatest/map pipes
 *   - Templates track signal reads automatically — no async pipe needed
 *   - Less boilerplate than NgRx for straightforward auth state
 *
 * Provided at root = exactly ONE instance shared across the entire app.
 * Every component and guard that injects AuthService gets the same signals.
 *
 * FLOW:
 *   1. app.config.ts registers loadMe() as an APP_INITIALIZER
 *   2. Angular calls it before bootstrapping any component
 *   3. loadMe() calls /auth/me using the session cookie
 *   4. Success → _me is set, user() returns the logged-in user
 *   5. Failure → _me stays undefined, user() returns null → guards redirect to /login
 */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  // ── Private writable signals ──────────────────────────────────────────────
  private readonly _loading = signal<boolean>(true); // True while calling /me endpoint
  private readonly _me = signal<MeDto | undefined>(undefined); // The call to get the authenticated user from backend

  // ── Public read-only signals ──────────────────────────────────────────────
  readonly loading = this._loading.asReadonly();
  readonly me = this._me.asReadonly();

  /**
   * Simplified user object for components that only need the username.
   *
   * computed() derives its value from _me automatically.
   * It recalculates only when _me changes, and is memoised —
   * reading it multiple times in one render cycle does not re-execute the function.
   */
  readonly user = computed(() => {
    const me = this._me();
    return me ? { username: me.username } : null;
  });

  // ── Data fetching ─────────────────────────────────────────────────────────

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
  async loadMe(): Promise<void> {
    this._loading.set(true);
    try {
      const me = await firstValueFrom(
        this.http.get<MeDto>('${environment.apiUrl}/auth/me', { withCredentials: true }),
      );
      localStorage.setItem('avatar', me.profilePicture ?? '');
      this._me.set({
        ...me,
        trips: me.trips ?? [],
      });
    } catch {
      this._me.set(undefined);
    } finally {
      this._loading.set(false);
    }
  }

  // ── Permission helpers ────────────────────────────────────────────────────

  canAccessTrip(trip: TripMembership): boolean {
    return this._me()?.trips?.includes(trip) ?? false;
  }
}
