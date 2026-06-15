import { computed, inject, Injectable, Signal, signal } from '@angular/core';
import { BaseApiService } from '../../../logic/services/base-api.service';
import { TripDto } from '../dtos/trip.dto';
import { TripMemberDto } from '../../dtos/trip/member.dto';

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

	//signal that stores the trip members so the form and dashboard share one source of truth
	private readonly membersSignal = signal<TripMemberDto[]>([]);
	//the members list that any component can read
	public members = this.membersSignal.asReadonly();

	//this signal looks to the tripSignal() to check if the trip is loaded - can be used to activate the spinner
	public isLoaded: Signal<boolean> = computed(() => this.tripSignal() !== null);

	//this method updates the trip info anywhere it is being loaded
	setTrip(trip: TripDto): void {
		this.tripSignal.set(trip);
	}

	//ver se é preciso fazer um update trip por causa de guardar despesas + itinerario

	// replaces the whole members list (e.g. after loading them from the backend)
	setMembers(members: TripMemberDto[]): void {
		this.membersSignal.set(members);
	}

	// appends a single member (e.g. after adding one through the dialog)
	addMember(member: TripMemberDto): void {
		this.membersSignal.update((list) => [...list, member]);
	}

	// updates the role of a single member
	updateMemberRole(userId: number, role: 'Admin' | 'Member'): void {
		this.membersSignal.update((list) =>
			list.map((m) => (m.userId === userId ? { ...m, role } : m)),
		);
	}

	// useful method to clear the state when the user leaves the trip dashboard
	clearTrip(): void {
		this.tripSignal.set(null);
		this.membersSignal.set([]);
	  }

}

