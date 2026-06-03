import { Component, inject, input, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators, FormBuilder } from '@angular/forms';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field'; // <-- Confirma esta linha
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { TranslocoModule } from '@jsverse/transloco';
import { LocationsService } from '../services/locations.service';
import { Observable, debounceTime, delay, distinctUntilChanged, map, of, switchMap, tap } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { TripVisibility } from '../dtos/trip.dto';

//Defining the date format
export const FORMAT_DMY = {
	parse: { dateInput: 'DD/MM/YYYY' },
	display: { dateInput: 'DD/MM/YYYY', monthYearLabel: 'MMM YYYY', dateA11yLabel: 'LL', monthYearA11yLabel: 'MMMM YYYY' },
};

@Component({
	selector: 'app-trip-form',
	imports: [AsyncPipe,
		MatFormFieldModule,
		TranslocoModule,
		MatDatepickerModule,
		MatNativeDateModule,
		FormsModule,
		ReactiveFormsModule,
		MatInputModule,
		MatAutocompleteModule,
		MatFormFieldModule,
		MatSelectModule,
	],
	templateUrl: './trip-form.component.html',
	styleUrl: './trip-form.component.scss',
	providers: [
		{ provide: MAT_DATE_LOCALE, useValue: 'pt-PT' },
		{ provide: MAT_DATE_FORMATS, useValue: FORMAT_DMY },
		provideNativeDateAdapter()
	]
})
export class TripFormComponent implements OnInit {

	tripGroup = input.required<FormGroup>();
	protected readonly TripVisibility = TripVisibility;

	private readonly locationsService = inject(LocationsService);
	listOfCountries$!: Observable<string[]>;
	private lastSuggestedCountries: string[] = [];
	listaPaises: string[] = ["Portugal", "Porto Rico", "Polónia"];

	ngOnInit(): void {

		// //a customized validator
		// this.country.addValidators(this.validateCountry());
		// this.country.updateValueAndValidity();


		this.listOfCountries$ = this.country.valueChanges.pipe(
			debounceTime(300),
			distinctUntilChanged(),
			switchMap((userInput: string | null) => { //muda de um observable para outro cancelando o que estiver em execuçao (O switch é o responsavel por cancelar o anterior caso outro observable seja emitido pelo 1º observable)
				const input = userInput || '';

				if (input.length < 2) {
					this.lastSuggestedCountries = [];
					return of([]);
				}
				return this.simulacaoBackend(input);
			})
		)
	}


	private simulacaoBackend(textoUser: string) {
		return of(this.listaPaises).pipe(
			delay(1000)
		)
	}

	// private validateCountry() {
	// 	return (control: AbstractControl): ValidationErrors | null => {
	// 		const valorEscrito = control.value;

	// 		if (!valorEscrito) return null;

	// 		// Verifica se o texto escrito bate com a propriedade 'name' de algum objeto da lista
	// 		const existeNaLista = this.lastSuggestedCountries.some(
	// 			(countryName: string) => countryName.toLowerCase() === valorEscrito.toLowerCase()
	// 		);

	// 		return !existeNaLista ? { invalidCountry: true } : null;
	// 	};
	// }

	get tripName() { return this.tripGroup().controls['tripName']; }
	get startDate() { return this.tripGroup().controls['startDate']; }
	get endDate() { return this.tripGroup().controls['endDate']; }
	get description() { return this.tripGroup().controls['description']; }
	get country() { return this.tripGroup().controls['country']; }
	get city() { return this.tripGroup().controls['city']; }
	get visibility() { return this.tripGroup().controls['visibility']; }
	get budget() { return this.tripGroup().controls['budget']; }
}
