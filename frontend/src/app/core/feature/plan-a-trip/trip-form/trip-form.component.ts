import { Component, inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field'; // <-- Confirma esta linha
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { LocationsService } from '../services/locations.service';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, of, switchMap, tap } from 'rxjs';

import { AsyncPipe } from '@angular/common';

//Defining the date format
export const FORMAT_DMY = {
	parse: { dateInput: 'DD/MM/YYYY' },
	display: { dateInput: 'DD/MM/YYYY', monthYearLabel: 'MMM YYYY', dateA11yLabel: 'LL', monthYearA11yLabel: 'MMMM YYYY' },
};

@Component({
	selector: 'app-trip-form',
	imports: [ AsyncPipe,
		MatFormFieldModule,
		TranslocoModule,
		MatDatepickerModule,
		MatNativeDateModule,
		FormsModule,
		ReactiveFormsModule,
		MatInputModule,
		MatAutocompleteModule,
		MatFormFieldModule
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

	private locationsService = inject(LocationsService);

	//date picker
	readonly range = new FormGroup({
		start: new FormControl<Date | null>(null),
		end: new FormControl<Date | null>(null),
	}, { validators: (control) => this.dateRangeValidator(control) });

	//country
	readonly countryControl = new FormControl<string>('', [Validators.required]);

	listOfCountries$!: Observable<string[]>;

	private lastSuggestedCountries: string[] = [];

	ngOnInit(): void {
        this.countryControl.setValidators([Validators.required, this.validateCountry()]);

        this.listOfCountries$ = this.countryControl.valueChanges.pipe(
            debounceTime(300),
            distinctUntilChanged(),
            switchMap(value => {
                const input = value || '';

                if (input.length < 2) {
                    this.lastSuggestedCountries = [];
                    return of([]);
                }

                return this.locationsService.searchCountries(input).pipe(
                    tap((response: any) => {
                        this.lastSuggestedCountries = response.data || [];
                    }),
                    // 3. CORREÇÃO DE SINTAXE AQUI: O operador || [] fica dentro do map
                    map((response: any) => response.data || [])
                );
            })
        );
    }

	private dateRangeValidator(range: AbstractControl) {

		const startDate = range.get('start')?.value;
		const endDate = range.get('end')?.value;

		if (startDate && endDate) {
			if (endDate <= startDate)
				return { wrongDates: true };
		}

		return null;
	}

	private validateCountry() {
		return (control: AbstractControl): ValidationErrors | null => {
			return (control.value && !this.lastSuggestedCountries.includes(control.value)) ? { invalidCountry: true } : null;
		}
	}



}
