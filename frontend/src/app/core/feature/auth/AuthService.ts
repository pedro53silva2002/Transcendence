import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../logic/services/base-api.service';
import { CreateUserDto } from './AuthDtos';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  public async register(dto: CreateUserDto): Promise<ApiResponse<void>> {
    return this._post<void>('/auth/register', dto);
  }

  /**
   *
   * @returns returns the Google URL for the user to be able to login with the Google account
   */
  public getGoogleRedirectUrl() {
    return this._getO<{ authorizationUrl: string }>('/auth/google/url');
  }
}
