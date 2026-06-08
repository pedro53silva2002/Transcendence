import { inject, Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { SearchParams, searchToQuery } from '../../../logic/services/search.service';
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

	searchCities(city: string, countryId: number): Observable<string[]> {
		const searchOptions: SearchParams<Record<string, unknown>, string> = {
			search: {
				name: { op: 'STARTSWITH', value: city },
				country_id: { op: 'EQUAL', value: countryId }
			},
		}
		const searchOptionsBase64 = searchToQuery(searchOptions);
		return this._get<{ content: CityDto[] }>(`/cities/search?q=${searchOptionsBase64}`).pipe(
			map(response => {
				const list = response?.data?.content || [];
				return list.map(city => city.name);
			})
		);
	}
}


