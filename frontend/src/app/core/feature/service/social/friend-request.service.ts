import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { Observable } from 'rxjs';
import { CreateFriendRequestDto, FriendRequestDto } from '../../dtos/social/friend-request.dto';
import { FriendshipDto } from '../../dtos/social/friendship.dto';

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

  public delete(id: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/friend-requests/${id}`);
    return res;
  }

  // FALTA UM GET PARA IR BUSCAR OS PEDIDOS
}
