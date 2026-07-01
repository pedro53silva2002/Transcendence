import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, BaseApiService } from './base-api.service';
import { CreateFriendRequestDto, FriendRequestDto } from '../dtos/friend-request.dto';
import { FriendshipDto } from '../dtos/friendship.dto';

@Injectable({ providedIn: 'root' })
export class FriendRequestService extends BaseApiService {
  public create(dto: CreateFriendRequestDto): Observable<ApiResponse<FriendRequestDto>> {
    const res = this._post<FriendRequestDto>(`/friend-requests`, dto);
    return res;
  }

  public accept(id: number): Observable<ApiResponse<FriendshipDto>> {
    const res = this._post<FriendshipDto>(`/friend-requests/${id}/accept`, id);
    return res;
  }

  public reject(id: number): Observable<ApiResponse<FriendshipDto>> {
    const res = this._post<FriendshipDto>(`/friend-requests/${id}/reject`, id);
    return res;
  }

  public delete(id: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/friend-requests/${id}`);
    return res;
  }

  public getAll(): Observable<ApiResponse<FriendRequestDto[]>> {
    return this._get<FriendRequestDto[]>('/friend-requests/received');
  }
}
