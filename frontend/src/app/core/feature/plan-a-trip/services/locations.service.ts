import { inject, Injectable } from '@angular/core';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';

@Injectable({
  providedIn: 'root',
})
export class LocationsService extends BaseApiService {
  
  searchCountries(country: string) {
    //return this._get<ApiResponse<string[]>>(`/countries/search?query=${country}`);
    return this._get<ApiResponse<string[]>>(`/countries/search?cXVlcnk9cG9ydHVnYWw=`);
  }

  searchCities(city: string, country: string) {
    return this._get<ApiResponse<string[]>>(`/cities/search?query=${city}&country=${country}`);
  }
}

//TENHO DE FAZER CODIFICAÇÃO 64????
