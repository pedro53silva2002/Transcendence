import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { CreateTripDto, ProfileTripsDto, TripDto, TripOrderByFieldsDto, TripSearchFieldsDto, UpdateTripDto } from '../dtos/trip.dto';
import { Observable } from 'rxjs';
import { CursorPage, SearchParams, searchToQuery } from '../../../logic/services/search.service';

@Injectable({
  providedIn: 'root',
})
export class TripService extends BaseApiService {

  //Create a trip (POST)
  public create(dto: CreateTripDto): Observable<ApiResponse<TripDto>> {
    return this._post<TripDto>(`/trips`, dto);
  }

  //Get trips (GET)
  public search(search: SearchParams<TripSearchFieldsDto, TripOrderByFieldsDto>): Observable<ApiResponse<CursorPage<TripDto>>> {
    const query = searchToQuery(search);
    return this._get<CursorPage<TripDto>>(`/trips/search?q=${query}`);
  }

  public searchTripsByUserId(userId: number): Observable<ApiResponse<ProfileTripsDto[]>> {
	return this._get<ProfileTripsDto[]>(`/trips/trip/${userId}`);
  }

  //Get trips by id (GET)
  public getById(id: number): Observable<ApiResponse<TripDto>> {
    return this._get<TripDto>(`/trips/${id}`);
  }

  //Update a trip (PUT)
  public update (id: number, dto: UpdateTripDto): Observable<ApiResponse<TripDto>> {
    return this._put<TripDto>(`/trips/${id}`, dto);
  }

  //Delete a trip (DELETE)
  public delete(id: number): Observable<ApiResponse<void>> {
    return this._delete<void>(`/trips/${id}`);
  }
}

