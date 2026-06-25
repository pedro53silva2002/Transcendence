<<<<<<< HEAD
import { CityDto } from "./city.dto";
import { CountryDto } from "./country.dto";

export enum TripVisibility {
    Private = 0,
    Public = 1,
    FriendsOnly = 2
=======
import { CreateTripMemberDto, TripMemberDto } from '../../dtos/trip/member.dto';
import { CityDto } from './city.dto';
import { CountryDto } from './country.dto';

export enum TripVisibility {
  Private = 0,
  Public = 1,
  FriendsOnly = 2,
>>>>>>> feature/trips
}

//what i send in http post request to create a trip
export interface CreateTripDto {
<<<<<<< HEAD
    tripName: string;
    description?: string;
    country: CountryDto;
	city?: CityDto[];
    startDate: string;
    endDate: string;
    budget: number;
    visibility: TripVisibility;
=======
  tripName: string;
  description?: string;
  country: CountryDto;
  city?: CityDto[];
  startDate: string;
  endDate: string;
  budget: number;
  visibility: TripVisibility;
  members: CreateTripMemberDto;
>>>>>>> feature/trips
}

//what i receive in http get
export interface TripDto {
<<<<<<< HEAD
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
=======
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
>>>>>>> feature/trips
}

//what i send in http put request to update a trip
export interface UpdateTripDto {
<<<<<<< HEAD
    id: number;
    tripName: string;
    description?: string;
    country: CountryDto;
	city?: CityDto[];
    startDate: string;
    endDate: string;
    budget: number;
    visibility: TripVisibility;
=======
  id: number;
  tripName: string;
  description?: string;
  country: CountryDto;
  city?: CityDto[];
  startDate: string;
  endDate: string;
  budget: number;
  visibility: TripVisibility;
>>>>>>> feature/trips
}

//what i receive in http get when i search for trips
export type TripSearchFieldsDto = {
<<<<<<< HEAD
    tripName?: string;
    country: CountryDto;
	city?: CityDto[];
}
=======
  tripName?: string;
  country: CountryDto;
  city?: CityDto[];
};
>>>>>>> feature/trips

export type TripOrderByFieldsDto = 'startDate' | 'createdAt' | 'tripName';
