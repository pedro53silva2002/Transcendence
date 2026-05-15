import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../environments/environment';
import type { MeDto, TripMembership } from '../dtos/me-dto.ts';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
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

  async loadMe(): Promise<void> {
    if (!this.tokenStorage.hasToken()) {
      this._loading.set(false);
      return;
    }

    this._loading.set(true);
    try {
      const me = await firstValueFrom(
        this.http.get<MeDto>(`${environment.apiUrl}/api/auth/me`),
      );
      this._me.set({ ...me, trips: me.trips ?? [] });
    } catch {
      this.tokenStorage.clearTokens();
      this._me.set(undefined);
    } finally {
      this._loading.set(false);
    }
  }

  clearSession(): void {
    this.tokenStorage.clearTokens();
    this._me.set(undefined);
    this._loading.set(false);
    this.router.navigate(['/']);
  }

  canAccessTrip(trip: TripMembership): boolean {
    return this._me()?.trips?.includes(trip) ?? false;
  }
}
