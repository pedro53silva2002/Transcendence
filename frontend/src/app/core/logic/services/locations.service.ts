import { inject, Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from './base-api.service';
import { SearchParams, searchToQuery } from './search.service';
import { HttpClient } from '@angular/common/http';
import { CountryDto } from '../dtos/country.dto';
import { map, Observable } from 'rxjs';
import { CityDto } from '../dtos/city.dto';


@Injectable({
	providedIn: 'root',
})
export class LocationsService extends BaseApiService {

	searchCountries(country: string): Observable<CountryDto[]> {
		const searchOptions: SearchParams<Record<string, unknown>, string> = {
			search: { name: { op: 'STARTSWITH', value: country } },
		}
		const searchOptionsBase64 = searchToQuery(searchOptions);
		return this._get<{ content: CountryDto[] }>(`/countries/search?q=${searchOptionsBase64}`).pipe(
			map(response => {
				return response?.data?.content || [];
			})
		);
	}

	searchCities(city: string, countryId: number): Observable<CityDto[]> {
		const searchOptions: SearchParams<Record<string, unknown>, string> = {
			search: {
				name: { op: 'STARTSWITH', value: city },
				country_id: { op: 'EQUAL', value: countryId }
			},
		}
		const searchOptionsBase64 = searchToQuery(searchOptions);
		return this._get<{ content: CityDto[] }>(`/cities/search?q=${searchOptionsBase64}`).pipe(
			map(response => {
				return response?.data?.content || [];
			})
		);
	}
}


