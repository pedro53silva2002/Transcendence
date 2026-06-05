import { Component, inject, OnInit } from '@angular/core';
import { TripFormComponent } from "./trip-form/trip-form.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TripService } from './services/trip.service';
import { CreateTripDto, TripDto, TripVisibility, UpdateTripDto } from './dtos/trip.dto';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TripStateService } from './services/trip-state.service';

@Component({
	selector: 'app-plan-a-trip',
	imports: [TripFormComponent, TranslocoModule, MatButtonModule, ReactiveFormsModule, RouterModule],
	templateUrl: './plan-a-trip.component.html',
	styleUrl: './plan-a-trip.component.scss',
})
export class PlanATripComponent implements OnInit {

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
			country: ['', [Validators.required]],
			city: [[] as string[]],
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

		console.log('Modo de Edição:', this.isEditMode);
		console.log('Dados no Estado:', this.tripStateService.trip());

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
				const dto: CreateTripDto = {
					tripName: formValue.trip.tripName,
					description: formValue.trip.description || undefined,
					country: formValue.trip.country,
					city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
					startDate: formValue.trip.startDate,
					endDate: formValue.trip.endDate,
					budget: formValue.trip.budget,
					visibility: formValue.trip.visibility,
					createdBy: 0, // ver com o diogo se precisamos disto
					members: formValue.crew.members
				};
	
				//just for testing if the tripStateService is storing the info about the created trip
				const tripDto: TripDto = {
					id: 120,
					tripName: formValue.trip.tripName,
					description: formValue.trip.description || undefined,
					country: formValue.trip.country,
					city: ["lisboa", "porto", "vila real"],
					// city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
					startDate: formValue.trip.startDate,
					endDate: formValue.trip.endDate,
					budget: formValue.trip.budget,
					visibility: formValue.trip.visibility,
					createdBy: 0, // ver com o diogo se precisamos disto
					createdAt: '',
					members: formValue.crew.members
				};
	
				this.tripStateService.setTrip(tripDto);
				this.router.navigate(['/trip-dashboard', tripDto.id]);

				// 	//http post to create trip
				// 	this.tripService.create(dto).subscribe({
				// 		next: (response) => {
				// 			if (response.data !== undefined)
				// 				this.tripStateService.setTrip(response.data);
				// 			console.log('Trip created!', response.data);
				// 			this.router.navigate(['/trip-dashboard', response.data?.id]);
				// 		},
				// 		error: (error) => {
				// 			console.error('Error creating trip:', error);
				// 		}
				// 	});
			} else {
				const tripId = this.route.snapshot.paramMap.get('id');

				//checks if tripId is valid
				if (tripId) {
					//builds the dto to send to backend
					const updateDto: UpdateTripDto = {
						id: Number(tripId),
						tripName: formValue.trip.tripName,
						description: formValue.trip.description || undefined,
						country: formValue.trip.country,
						city: formValue.trip.city,
						startDate: formValue.trip.startDate,
						endDate: formValue.trip.endDate,
						budget: formValue.trip.budget,
						visibility: formValue.trip.visibility,
					}

					//just for testing if the tripStateService is storing the info about the updated trip
					const tripDto: TripDto = {
					id: 120,
					tripName: formValue.trip.tripName,
					description: formValue.trip.description || undefined,
					country: formValue.trip.country,
					city: ["lisboa", "porto", "vila real"],
					// city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
					startDate: formValue.trip.startDate,
					endDate: formValue.trip.endDate,
					budget: formValue.trip.budget,
					visibility: formValue.trip.visibility,
					createdBy: 0, // ver com o diogo se precisamos disto
					createdAt: '',
					members: formValue.crew.members
				};

				this.tripStateService.setTrip(tripDto);
				this.router.navigate(['/trip-dashboard', tripDto.id]);
	
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


}
