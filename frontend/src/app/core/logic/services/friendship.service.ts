import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from './base-api.service';
import { CreateFriendshipDto, FriendDto, FriendshipDto } from '../dtos/friendship.dto';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FriendshipService extends BaseApiService {
  public create(dto: CreateFriendshipDto): Observable<ApiResponse<FriendshipDto>> {
    const res = this._post<FriendshipDto>(`/friendships`, dto);
    return res;
  }

  public getAll(): Observable<ApiResponse<FriendDto[]>> {
    const res = this._get<FriendDto[]>(`/friendships`);
    return res;
  }

  public count(): Observable<ApiResponse<number>> {
    const res = this._get<number>(`friendships/count`);
    return res;
  }

  public getById(id: number): Observable<ApiResponse<FriendshipDto>> {
    const res = this._get<FriendshipDto>(`/friendships/${id}`);
    return res;
  }

  public delete(id: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/friendships/${id}`);
    return res;
  }
}
