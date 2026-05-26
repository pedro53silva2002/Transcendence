import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../logic/services/base-api.service';
import { CreateUserDto, MeDto, UserDto } from './AuthDtos';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  public register(dto: CreateUserDto): Observable<ApiResponse<UserDto>> {
    return this._post<UserDto>('/auth/register', dto);
  }

  public me(): Observable<ApiResponse<MeDto>> {
    return this._getO<MeDto>('/auth/me');
  }
}
