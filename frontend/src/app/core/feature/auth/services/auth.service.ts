import { inject, Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { AuthResponseDto, LoginDto, MeDto } from '../dtos/auth.dto';
import { CreateUserDto } from '../dtos/user.dto';
import { SessionService } from '../../../logic/services/session.service';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  private readonly authSession = inject(SessionService);
  public register(dto: CreateUserDto): Observable<ApiResponse<AuthResponseDto>> {
    return this._post<AuthResponseDto>('/auth/register', dto);
  }

  public me(): Observable<ApiResponse<MeDto>> {
    return this._get<MeDto>('/auth/me');
  }

  public login(dto: LoginDto): Observable<ApiResponse<AuthResponseDto>> {
    return this._post<AuthResponseDto>('/auth/login', dto);
  }

  /**
   *
   * @returns returns the Google URL for the user to be able to login with the Google account
   */
  public getGoogleRedirectUrl() {
    return this._getO<{ authorizationUrl: string }>('/auth/google/url');
  }

  public logout(): Observable<ApiResponse<void>> {
    const res = this._post<void>(`/auth/logout`, null);
    this.authSession.clearSession();
    return res;
  }
}
