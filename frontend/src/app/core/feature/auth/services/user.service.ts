import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import {
  CreateUserDto,
  UpdateUserDto,
  UserDto,
  UserOrderByFieldsDto,
  UserSearchFieldsDto,
} from '../dtos/user.dto';
import { CursorPage, SearchParams, searchToQuery } from '../../../logic/services/search.service';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseApiService {
  public create(dto: CreateUserDto): Observable<ApiResponse<UserDto>> {
    return this._post<UserDto>(`/users`, dto);
  }

  public search(
    search: SearchParams<UserSearchFieldsDto, UserOrderByFieldsDto>,
  ): Observable<ApiResponse<CursorPage<UserDto>>> {
    const query = searchToQuery(search);
    return this._get<CursorPage<UserDto>>(`/users/search?q=${query}`);
  }

  public update(dto: UpdateUserDto): Observable<ApiResponse<UserDto>> {

    const formData = new FormData();

    formData.append('Email', dto.email);
    formData.append('Username', dto.username);
    formData.append('DisplayName', dto.displayName);

    if (dto.bio) {
      formData.append('Bio', dto.bio);
    }

    if (dto.password) {
      formData.append('Password', dto.password);
    }

    if (dto.profilePhotoUrl instanceof File) {
      formData.append('ProfilePhotoUrl', dto.profilePhotoUrl, dto.profilePhotoUrl.name);
    }

    return this._put<UserDto>(`/users`, formData as any);
  }

  public delete(): Observable<ApiResponse<void>> {
    return this._delete<void>(`/users`);
  }
}
