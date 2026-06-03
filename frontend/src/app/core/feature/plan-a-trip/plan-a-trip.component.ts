import { Component, inject } from '@angular/core';
import { TripFormComponent } from "./trip-form/trip-form.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TripService } from './services/trip.service';
import { CreateTripDto, TripVisibility } from './dtos/trip.dto';

@Component({
	selector: 'app-plan-a-trip',
	imports: [TripFormComponent, TranslocoModule, MatButtonModule, ReactiveFormsModule],
	templateUrl: './plan-a-trip.component.html',
	styleUrl: './plan-a-trip.component.scss',
})
export class PlanATripComponent {
	private readonly formBuilder = inject(FormBuilder);
	private readonly tripService = inject(TripService);

	planATripForm = this.formBuilder.nonNullable.group({
		trip: this.formBuilder.nonNullable.group({
			tripName: ['', [Validators.required, Validators.maxLength(25), Validators.minLength(3)]],
			description: ['', [Validators.maxLength(256)]],
			country: [[] as string[], [Validators.required]],
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

	submitTrip() {

		if (this.planATripForm.valid) {
			const formValue = this.planATripForm.getRawValue();

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

			this.tripService.create(dto).subscribe({
				next: (response) => {
					console.log('Viagem criada com sucesso!', response.data);
					//navegar para a dashboard da trip criada
				},
				error: (error) => {
					console.error('Erro ao criar viagem:', error);
				}
			});
		}

	}
}
