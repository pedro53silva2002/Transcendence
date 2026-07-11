import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import type { MeDto, TripMembershipDto } from '../dtos/auth.dto';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class SessionService {
  private readonly apiURL = `${(environment as any).apiUrl}`;
  private readonly http = inject(HttpClient);
  private readonly tokenStorage = inject(TokenStorageService);

  private readonly _loading = signal<boolean>(true);
  private readonly _me = signal<MeDto | undefined>(undefined);

  readonly loading = this._loading.asReadonly();
  readonly me = this._me.asReadonly();

  private isOAuthSuccessful = true;

  readonly user = computed(() => {
    const me = this._me();
    return me ? { username: me.username } : null;
  });

  loadMe(): Observable<boolean> {
    //to prevent loadMe() to work if there's no one authenticated, like in the landing page
    const hasToken = !!this.tokenStorage.getAccessToken();
    if (!hasToken) {
      this._me.set(undefined);
      this._loading.set(false);
      return of(false);
    }
    return this.http.get<MeDto>(`${this.apiURL}/auth/me`).pipe(
      tap((user) => {
        this._me.set(user);
        this._loading.set(false);
      }),
      map(() => true),
      catchError(() => {
        this._me.set(undefined);
        this._loading.set(false);
        return of(false);
      }),
    );
  }

  clearSession(): void {
    this._me.set(undefined);
    this.tokenStorage.clearTokens();
  }

  isAuthenticated(): boolean {
    return this._me() !== undefined;
  }

  canAccessTrip(trip: TripMembershipDto): boolean {
    return this._me()?.trips?.includes(trip) ?? false;
  }

  getOAuthResult(): boolean {
    return this.isOAuthSuccessful;
  }

  setOAuthSuccess(result: boolean): void {
    this.isOAuthSuccessful = result;
  }
}
