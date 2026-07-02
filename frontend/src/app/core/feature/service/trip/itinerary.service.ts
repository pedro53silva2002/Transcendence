import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import {
  CreateItineraryDto,
  ItineraryDto,
  ItineraryOrderByDto,
  ItinerarySearchFieldsDto,
} from '../../itinerary/dtos/itinerary.dto';
import { Observable } from 'rxjs';
import { CursorPage, SearchParams, searchToQuery } from '../../../logic/services/search.service';

@Injectable({ providedIn: 'root' })
export class ItineraryService extends BaseApiService {
  public create(dto: CreateItineraryDto): Observable<ApiResponse<ItineraryDto>> {
    const res = this._post<ItineraryDto>(`/trips/itinerary`, dto);
    return res;
  }

  public search(
    search: SearchParams<ItinerarySearchFieldsDto, ItineraryOrderByDto>,
  ): Observable<ApiResponse<CursorPage<ItineraryDto>>> {
    const query = searchToQuery(search);
    const res = this._get<CursorPage<ItineraryDto>>(`/trips/itinerary/search?q=${query}`);

    return res;
  }

  public delete(id: number): Observable<ApiResponse<void>> {
    const res = this._delete<void>(`/trips/itinerary/${id}`);
    return res;
  }
}
