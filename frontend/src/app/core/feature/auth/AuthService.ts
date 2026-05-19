import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../logic/services/base-api.service';
import { CreateUserDto, MeDto, UserDto } from './AuthDtos';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  public async register(dto: CreateUserDto): Promise<ApiResponse<UserDto>> {
    return this._post<UserDto>('/auth/register', dto);
  }

  public async me(): Promise<ApiResponse<MeDto>> {
    return this._get<MeDto>('/auth/me');
  }
}
