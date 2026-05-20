import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../logic/services/base-api.service';
import { AuthResponseDto, CreateUserDto, LoginDto, MeDto, UserDto } from './AuthDtos';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
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
}
