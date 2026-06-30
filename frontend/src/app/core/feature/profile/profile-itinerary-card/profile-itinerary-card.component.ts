import { Component, computed, inject, input } from '@angular/core';
import { ProfileTripsDto } from '../../plan-a-trip/dtos/trip.dto';
import { DatePipe, UpperCasePipe } from '@angular/common';
import { CityDto } from '../../plan-a-trip/dtos/city.dto';
import { CountryDto } from '../../plan-a-trip/dtos/country.dto';
import { MatDialog } from '@angular/material/dialog';
import ItineraryComponent from '../../itinerary/itinerary.component';

@Component({
	selector: 'app-profile-itinerary-card',
	imports: [DatePipe, UpperCasePipe, ItineraryComponent],
	templateUrl: './profile-itinerary-card.component.html',
	styleUrl: './profile-itinerary-card.component.scss',
})
export class ProfileItineraryCardComponent {

	private readonly dialog = inject(MatDialog);

	showAddButton = input<boolean>(true);
	itinerary = input.required<ProfileTripsDto>();

	readonly imagePath = computed(() => getSeasonImage(this.itinerary().startDate));


	openItinerary(): void {

		this.dialog.open(ItineraryComponent, {
			width: '660px',
			data: {
				itinerary: this.itinerary(),
				showAddButton: false,
				profileRoute: true,
				showAvatar: false,
				showDeleteButton: false,
			}
		});
	}

}

function getSeasonImage(startDate: string): string {
	const month = new Date(startDate).getMonth() + 1;
	if (month >= 3 && month <= 5) return '/images/spring.png';
	if (month >= 6 && month <= 8) return '/images/summer.png';
	if (month >= 9 && month <= 11) return '/images/autumn.png';
	return '/images/winter.png';
}