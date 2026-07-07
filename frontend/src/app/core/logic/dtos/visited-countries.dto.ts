import { DateTime } from "luxon";
import { CountryDto } from "./country.dto";

export interface VisitedCountryDto {
	id: number;
	country: CountryDto;
	addedAt: DateTime;
	deletedAt: DateTime;
	sourceTripId: number;
}
