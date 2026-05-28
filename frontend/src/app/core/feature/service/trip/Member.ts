import {
  CreateTripMemberDto,
  TripMemberDto,
  TripMemberOrderByDto,
  TripMemberSearchFieldsDto,
  UpdateTripMemberDto,
} from './../../dtos/trip/Member';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { Observable } from 'rxjs';
import { buildSearchUrl, SearchParams } from '../../../logic/services/search.service';

export class TripMemberService extends BaseApiService {
  public create(dto: CreateTripMemberDto): Observable<ApiResponse<TripMemberDto>> {
    const res = this._post<TripMemberDto>(`/trips/members`, dto);
    return res;
  }

  public search(
    search: SearchParams<TripMemberSearchFieldsDto, TripMemberOrderByDto>,
  ): Observable<ApiResponse<TripMemberDto[]>> {
    const res = this._get<TripMemberDto[]>(buildSearchUrl(`/trips/members/search`, search));

    return res;
  }

  public update(id: number, dto: UpdateTripMemberDto): Observable<ApiResponse<TripMemberDto>> {
    const res = this._put<TripMemberDto>(`/trips/members/${id}`, dto);

    return res;
  }

  public delete(id: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/trips/members/${id}`);
    return res;
  }
}
