import { inject, Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { AuthResponseDto, LoginDto, MeDto } from './../dtos/AuthDtos';
import { CreateUserDto } from '../dtos/UserDto';
import { AuthService as SessionService } from '../../../logic/services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  private readonly authSession = inject(SessionService);
  public async register(dto: CreateUserDto): Promise<ApiResponse<AuthResponseDto>> {
    return this._post<AuthResponseDto>('/auth/register', dto);
  }

  public async me(): Promise<ApiResponse<MeDto>> {
    return this._get<MeDto>('/auth/me');
  }

  public async login(dto: LoginDto): Promise<ApiResponse<AuthResponseDto>> {
    return this._post<AuthResponseDto>('/auth/login', dto);
  }

  /**
   *
   * @returns returns the Google URL for the user to be able to login with the Google account
   */
  public getGoogleRedirectUrl() {
    return this._getO<{ authorizationUrl: string }>('/auth/google/url');
  }

  public async logout(): Promise<ApiResponse<void>> {
    const res =  await this._post<void>(`/auth/logout`, null);
    this.authSession.clearSession();
    return res;
  }
}
