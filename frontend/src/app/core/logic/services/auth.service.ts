import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import type { MeDto, TripMembership } from '../dtos/me-dto.ts';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  getGoogleRedirectUrl() {
    throw new Error('Method not implemented.');
  }
  private readonly apiURL = `${(environment as any).apiUrl}`;

  fetchCurrentUser() {
    throw new Error('Method not implemented.');
  }
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
}
