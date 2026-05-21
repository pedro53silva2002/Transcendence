import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import {
  CreateUserDto,
  UserDto,
  UserOrderByFieldsDto,
  UserSearchFieldsDto,
} from '../dtos/user.dto';
import { CursorPage, SearchParams, searchToQuery } from '../../../logic/services/search.service';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseApiService {
  public async create(dto: CreateUserDto): Promise<ApiResponse<UserDto>> {
    const res = await this._post<UserDto>(`/users`, dto);
    return res;
  }
  public async search(
    search: SearchParams<UserSearchFieldsDto, UserOrderByFieldsDto>,
  ): Promise<ApiResponse<CursorPage<UserDto>>> {
    const query = searchToQuery(search);
    const res = this._get<CursorPage<UserDto>>(`/users/search?q=${query}`);

    return res;
  }

  public async delete(id: number): Promise<ApiResponse<void>> {
    const res = this._delete<void>(`/users`);
    return res;
  }
}
