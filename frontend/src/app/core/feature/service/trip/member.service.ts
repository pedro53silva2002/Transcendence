import { CursorPage } from '../../../logic/services/search.service';
import {
  CreateTripMemberDto,
  TripMemberDto,
  TripMemberOrderByDto,
  TripMemberSearchFieldsDto,
  UpdateTripMemberDto,
} from '../../dtos/trip/member.dto';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  buildSearchUrl,
  SearchParams,
  searchToQuery,
} from '../../../logic/services/search.service';

@Injectable({ providedIn: 'root' })
export class TripMemberService extends BaseApiService {
  public create(dto: CreateTripMemberDto, tripId: number): Observable<ApiResponse<TripMemberDto>> {
    const res = this._post<TripMemberDto>(`/trips/${tripId}/members`, dto);
    return res;
  }

  public search(
    search: SearchParams<TripMemberSearchFieldsDto, TripMemberOrderByDto>,
    tripId: number,
  ): Observable<ApiResponse<CursorPage<TripMemberDto>>> {
    const query = searchToQuery(search);
    const res = this._get<CursorPage<TripMemberDto>>(`/trips/${tripId}/members/search?q=${query}`);

    return res;
  }

  public update(
    id: number,
    dto: UpdateTripMemberDto,
    tripId: number,
  ): Observable<ApiResponse<TripMemberDto>> {
    const res = this._put<TripMemberDto>(`/trips/${tripId}/members/${id}`, dto);

    return res;
  }

  public delete(id: number, tripId: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/trips/${tripId}/members/${id}`);
    return res;
  }
}
