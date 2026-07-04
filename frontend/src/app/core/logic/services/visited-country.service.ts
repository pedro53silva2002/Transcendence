import { Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from './base-api.service';
import { VisitedCountryDto } from '../dtos/visited-countries.dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VisitedCountryService extends BaseApiService{

	public getVisitedCountries(userId: number): Observable<ApiResponse<VisitedCountryDto>> {
		return this._get<VisitedCountryDto>(`/visited-countries/${userId}`);
	}

	public getCountVisitedCountries(userId: number): Observable<ApiResponse<number>> {
		return this._get<number>(`/visited-countries/${userId}/count`);
	}
}
