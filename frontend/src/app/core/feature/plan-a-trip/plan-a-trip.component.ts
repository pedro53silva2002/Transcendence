import { Component, inject, OnInit, viewChild } from '@angular/core';
import { TripFormComponent } from "./trip-form/trip-form.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { TripService } from './services/trip.service';
import { CreateTripDto, TripVisibility, UpdateTripDto } from './dtos/trip.dto';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TripStateService } from './services/trip-state.service';
import { CountryDto } from './dtos/country.dto';
import { CityDto } from './dtos/city.dto';

@Component({
	selector: 'app-plan-a-trip',
	imports: [TripFormComponent, TranslocoModule, MatButtonModule, ReactiveFormsModule, RouterModule],
	templateUrl: './plan-a-trip.component.html',
	styleUrl: './plan-a-trip.component.scss',
})
export class PlanATripComponent implements OnInit {

	tripForm = viewChild(TripFormComponent);

	private readonly formBuilder = inject(FormBuilder);
	private readonly tripService = inject(TripService);
	private readonly router = inject(Router);
	private readonly tripStateService = inject(TripStateService);
	private readonly route = inject(ActivatedRoute);
	
	public isEditMode = false;

	planATripForm = this.formBuilder.nonNullable.group({
		trip: this.formBuilder.nonNullable.group({
			tripName: ['', [Validators.required, Validators.maxLength(25), Validators.minLength(3)]],
			description: ['', [Validators.maxLength(250)]],
			country: [ null as CountryDto | null, [Validators.required, PlanATripComponent.countryValidator]],
			city: [[] as CityDto[] , [PlanATripComponent.cityValidator]],
			startDate: ['', [Validators.required]],
			endDate: ['', [Validators.required]],
			budget: [null as unknown as number, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]], // Aceita apenas números inteiros
			visibility: [TripVisibility.Private, [Validators.required]]
		}),
		crew: this.formBuilder.nonNullable.group({
			members: [[] as number[]]
		})
	});

	ngOnInit(): void {
		this.isEditMode = this.router.url.includes('edit');

		if (this.isEditMode) {
			const currentTrip = this.tripStateService.trip(); //reads the value of the tripStateService

			if (currentTrip) {
				this.planATripForm.patchValue({
					trip: {
						tripName: currentTrip.tripName,
						description: currentTrip.description,
						country: currentTrip.country,
						city: currentTrip.city,
						startDate: currentTrip.startDate,
						endDate: currentTrip.endDate,
						budget: currentTrip.budget,
						visibility: currentTrip.visibility
					}
					//add here crew members later
				});
			}
		}
	}

	submitTrip() {

		if (this.planATripForm.valid) {
			const formValue = this.planATripForm.getRawValue();

			if (!this.isEditMode) {
				if (formValue.trip.country) {
					console.log(formValue.trip.city.map(( cities => cities.id )));
					const dto: CreateTripDto = {
						tripName: formValue.trip.tripName,
						description: formValue.trip.description || undefined,
						country: formValue.trip.country,
						city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
						startDate: formValue.trip.startDate,
						endDate: formValue.trip.endDate,
						budget: formValue.trip.budget,
						visibility: formValue.trip.visibility,
						// createdBy: 1, // ver com o diogo se precisamos disto, não é arriscado este parâmetro não ser identificado no backend?
						// members: formValue.crew.members
					};

					//http post to create trip
					this.tripService.create(dto).subscribe({
						next: (response) => {
							if (response.data !== undefined)
								this.tripStateService.setTrip(response.data);
							console.log('Trip created!', response.data);
							this.router.navigate(['/trip-dashboard', response.data?.id]);
						},
						error: (error) => {
							console.error('Error creating trip:', error);
						}
					});
				}
	
				//just for testing if the tripStateService is storing the info about the created trip
				// if (formValue.trip.country) {
				// 	const tripDto: TripDto = {
				// 		id: 120,
				// 		tripName: formValue.trip.tripName,
				// 		description: formValue.trip.description || undefined,
				// 		country: formValue.trip.country,
				// 		city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
				// 		startDate: formValue.trip.startDate,
				// 		endDate: formValue.trip.endDate,
				// 		budget: formValue.trip.budget,
				// 		visibility: formValue.trip.visibility,
				// 		createdBy: 0, // ver com o diogo se precisamos disto
				// 		createdAt: '',
				// 		members: formValue.crew.members
				// 	};
				// 	this.tripStateService.setTrip(tripDto);
				// 	this.router.navigate(['/trip-dashboard', tripDto.id]);
				// }


			} else {
				const tripId = this.route.snapshot.paramMap.get('id');

				//checks if tripId is valid
				if (tripId && formValue.trip.country) {
					//builds the dto to send to backend
						const updateDto: UpdateTripDto = {
							id: Number(tripId),
							tripName: formValue.trip.tripName,
							description: formValue.trip.description || undefined,
							country: formValue.trip.country,
							city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
							startDate: formValue.trip.startDate,
							endDate: formValue.trip.endDate,
							budget: formValue.trip.budget,
							visibility: formValue.trip.visibility,
						}

					//just for testing if the tripStateService is storing the info about the updated trip
					// if (formValue.trip.country) {
					// 	const tripDto: TripDto = {
					// 	id: 120,
					// 	tripName: formValue.trip.tripName,
					// 	description: formValue.trip.description || undefined,
					// 	country: formValue.trip.country,
					// 	city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
					// 	startDate: formValue.trip.startDate,
					// 	endDate: formValue.trip.endDate,
					// 	budget: formValue.trip.budget,
					// 	visibility: formValue.trip.visibility,
					// 	createdBy: 0, // ver com o diogo se precisamos disto
					// 	createdAt: '',
					// 	members: formValue.crew.members
					// };
	
					// this.tripStateService.setTrip(tripDto);
					// this.router.navigate(['/trip-dashboard', tripDto.id]);
					// }
	
					this.tripService.update(updateDto.id, updateDto).subscribe({
						next: (response) => {
							if (response.data !== undefined)
								this.tripStateService.setTrip(response.data);
							this.router.navigate(['/trip-dashboard', response.data?.id]);
						},
						error: (error) => {
							console.error('Error updating trip:', error);
						}
					})
				}

			}
		}
	}

	static countryValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value;
		
		if (!value)
			return null;

		if (typeof value === 'string') {
			return {countryNotSelected: true};
		}

		if (typeof value === 'object' && !value.id)	{
			return { countryNotSelected: true };
		}
		return null;
	}

	static cityValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value;

		if (!value || (Array.isArray(value) && value.length === 0) || (typeof value === 'string' && value.trim() === '')) {
			return null;
		}

		if (typeof value === 'string') {
			return { cityNotSelected: true };
		}
		return null;
	}
}
