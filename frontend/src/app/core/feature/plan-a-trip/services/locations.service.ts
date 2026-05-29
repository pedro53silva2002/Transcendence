import { inject, Injectable } from '@angular/core';
import { BaseApiService } from '../../../logic/services/base-api.service';

@Injectable({
  providedIn: 'root',
})
export class LocationsService extends BaseApiService {
  
  searchCountries(country: string) {
    return this._get<string[]>(`${this.apiUrl}/countries?search=${country}`);
  }

  searchCities(city: string, country: string) {
    return this._get<string[]>(`${this.apiUrl}/cities?search=${city}&country=${country}`);
  }
}
