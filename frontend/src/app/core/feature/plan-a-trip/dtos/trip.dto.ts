import { CreateTripMemberDto, TripMemberDto } from '../../dtos/trip/member.dto';
import { CityDto } from './city.dto';
import { CountryDto } from './country.dto';

export enum TripVisibility {
  Private = 0,
  Public = 1,
  FriendsOnly = 2,
}

//what i send in http post request to create a trip
export interface CreateTripDto {
  tripName: string;
  description?: string;
  country: CountryDto;
  city?: CityDto[];
  startDate: string;
  endDate: string;
  budget: number;
  visibility: TripVisibility;
  members: CreateTripMemberDto;
}

//what i receive in http get
export interface TripDto {
  id: number;
  tripName: string;
  description?: string;
  country: CountryDto;
  city?: CityDto[];
  startDate: string;
  endDate: string;
  budget: number;
  visibility: TripVisibility;
  createdBy: number;
  createdAt: string;
  updatedAt?: string;
}

//what i send in http put request to update a trip
export interface UpdateTripDto {
  id: number;
  tripName: string;
  description?: string;
  country: CountryDto;
  city?: CityDto[];
  startDate: string;
  endDate: string;
  budget: number;
  visibility: TripVisibility;
}

//what i receive in http get when i search for trips
export type TripSearchFieldsDto = {
  tripName?: string;
  country: CountryDto;
  city?: CityDto[];
};

export type TripOrderByFieldsDto = 'startDate' | 'createdAt' | 'tripName';
