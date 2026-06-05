import { computed, inject, Injectable, Signal, signal } from '@angular/core';
import { BaseApiService } from '../../../logic/services/base-api.service';
import { TripDto } from '../dtos/trip.dto';

/* This service will store the info od the trip so we 
	don't need to keep making requests to the backend
*/

@Injectable({
	providedIn: 'root',
})
export class TripStateService {

	//signal that will store the state of the Trip (can have the object TripDto or be null)
	private readonly tripSignal = signal<TripDto | null>(null);
	//the trip information that any component can read
	public trip = this.tripSignal.asReadonly();

	//this signal looks to the tripSignal() to check if the trip is loaded - can be used to activate the spinner
	public isLoaded: Signal<boolean> = computed(() => this.tripSignal() !== null);

	//this method updates the trip info anywhere it is being loaded
	setTrip(trip: TripDto): void {
		this.tripSignal.set(trip);
	}

	//ver se é preciso fazer um update trip por causa de guardar despesas + itinerario

	// useful method to clear the state when the user leaves the trip dashboard
	clearTrip(): void {
		this.tripSignal.set(null);
	  }

}

