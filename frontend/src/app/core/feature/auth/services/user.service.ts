import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import {
  CreateUserDto,
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

  public delete(id: number): Observable<ApiResponse<void>> {
    return this._delete<void>(`/users/${id}`);
  }
}
