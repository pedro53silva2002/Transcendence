import { Component, ElementRef, inject, Injectable, input, OnInit, signal, viewChild } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, NativeDateAdapter, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { TranslocoModule } from '@jsverse/transloco';
import { LocationsService } from '../../../logic/services/locations.service';
import { Observable, debounceTime, delay, distinctUntilChanged, map, of, switchMap, tap } from 'rxjs';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { TripVisibility } from '../../../logic/dtos/trip.dto';
import { CountryDto } from '../../../logic/dtos/country.dto';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { CityDto } from '../../../logic/dtos/city.dto';
import { DateTime } from 'luxon';

//Defining the date format
export const FORMAT_DMY = {
	parse: { dateInput: 'DD/MM/YYYY' },
	display: { dateInput: 'DD/MM/YYYY', monthYearLabel: 'MMM YYYY', dateA11yLabel: 'LL', monthYearA11yLabel: 'MMMM YYYY' },
};


@Injectable()
export class PlainDateAdapter extends NativeDateAdapter {
	// Altera a forma como o Angular Material serializa a data para o formulário
	override toIso8601(date: Date): string {
		// Retorna rigorosamente YYYY-MM-DD no fuso horário local, sem horas
		return DateTime.fromJSDate(date).toISODate()!;
	}
}

@Component({
	selector: 'app-trip-form',
	standalone: true,
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
		MatChipsModule,
		MatIconModule,
	],
	templateUrl: './trip-form.component.html',
	styleUrl: './trip-form.component.scss',
	providers: [
		{ provide: MAT_DATE_LOCALE, useValue: 'pt-PT' },
		{ provide: MAT_DATE_FORMATS, useValue: FORMAT_DMY },
		{ provide: DateAdapter, useClass: PlainDateAdapter }
	]
})
export class TripFormComponent implements OnInit {

	tripGroup = input.required<FormGroup>();
	protected readonly TripVisibility = TripVisibility;

	private readonly locationsService = inject(LocationsService);
	listOfCountries$!: Observable<CountryDto[]>;
	listOfCities$!: Observable<CityDto[]>;

	cityInput = viewChild<ElementRef<HTMLInputElement>>('cityInput');

	selectedCountryId = signal<number | null>(null);
	selectedCities = signal<CityDto[]>([]);

	ngOnInit(): void {

		//check if country already exists in the form
		const existingCountry = this.country.value;
		if (existingCountry) {
			this.selectedCountryId.set(existingCountry.id);
		}

		//check if cities are already filled in the form
		const existingCities = this.city.value;
		if (Array.isArray(existingCities)) {
			this.selectedCities.set(existingCities);
		}

		this.listOfCountries$ = this.country.valueChanges.pipe(
			debounceTime(300),
			distinctUntilChanged(),
			switchMap((userInput: string | null) => { //muda de um observable para outro cancelando o que estiver em execuçao (O switch é o responsavel por cancelar o anterior caso outro observable seja emitido pelo 1º observable)
				const input = userInput || '';

				if (input.length < 2) {
					this.selectedCountryId.set(null);
					return of([]);
				}
				return this.locationsService.searchCountries(input);
			})
		);

		this.listOfCities$ = this.city.valueChanges.pipe(
			debounceTime(300),
			distinctUntilChanged(),
			switchMap((userInput: any) => {
				const input = userInput || '';

				const countryId = this.selectedCountryId();

				if (input.length < 1 || !countryId) {
					return of([]);
				}
				return this.locationsService.searchCities(input, countryId);
			})
		)
	}

	//when a user selects a country
	onCountrySelected(country: CountryDto): void {
		this.selectedCountryId.set(country.id);

		//cleans the cities to prevent using wrong cities for the country
		this.selectedCities.set([]);
		this.city.setValue('');
	}

	//when a user selects a city
	selectedCity(event: MatAutocompleteSelectedEvent): void {
		const cityValue = event.option.value;

		if (cityValue && !this.selectedCities().includes(cityValue)) {
			this.selectedCities.update(cities => {
				const updated = [...cities, cityValue];
				this.city.setValue(updated, { emitEvent: false });
				console.log(this.city.value);
				return updated;
			})
		}

		//cleans the input text for next search
		const inputEl = this.cityInput()?.nativeElement;
		if (inputEl) {
			inputEl.value = '';
		}
	}

	//when a user clicks to remove a city
	removeCity(cityName: string): void {
		this.selectedCities.update(cities => {
			const updated = cities.filter(city => city.name !== cityName);
			this.city.setValue(updated, { emitEvent: false }); //to prevent it from triggering an API request
			return updated;
		});
	}

	//to customize the display of the country in the autocomplete input
	displayCountryFn(country: CountryDto | null): string {
		return country && country.name ? country.name : '';
	}

	public updateFormFields(cities: CityDto[], visibility: TripVisibility, countryId?: number | null): void {
		this.selectedCities.set(cities ?? []);
		if (countryId) {
			this.selectedCountryId.set(countryId);
		}

		if (visibility !== undefined) {
			this.visibility.setValue(visibility, { emitEvent: false });
		}
	}

	get tripName() { return this.tripGroup().controls['tripName']; }
	get startDate() { return this.tripGroup().controls['startDate']; }
	get endDate() { return this.tripGroup().controls['endDate']; }
	get description() { return this.tripGroup().controls['description']; }
	get country() { return this.tripGroup().controls['country']; }
	get city() { return this.tripGroup().controls['city']; }
	get visibility() { return this.tripGroup().controls['visibility']; }
	get budget() { return this.tripGroup().controls['budget']; }
}
