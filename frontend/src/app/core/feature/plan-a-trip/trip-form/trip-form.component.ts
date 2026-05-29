import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn } from '@angular/forms';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from '@angular/material/input';
import { TranslocoModule } from '@jsverse/transloco';

//Defining the date format
export const FORMAT_DMY = {
	parse: {
	  dateInput: 'DD/MM/YYYY',
	},
	display: {
	  dateInput: 'DD/MM/YYYY',
	  monthYearLabel: 'MMM YYYY',
	  dateA11yLabel: 'LL',
	  monthYearA11yLabel: 'MMMM YYYY',
	},
  };

@Component({
	selector: 'app-trip-form',
	imports: [MatFormFieldModule,
				TranslocoModule,
				MatDatepickerModule,
				MatNativeDateModule,
				FormsModule,
				ReactiveFormsModule,
				MatInputModule
				],
	templateUrl: './trip-form.component.html',
	styleUrl: './trip-form.component.scss',
	providers: [
		{ provide: MAT_DATE_LOCALE, useValue: 'pt-PT' },
		{ provide: MAT_DATE_FORMATS, useValue: FORMAT_DMY },
		provideNativeDateAdapter()
	  ]
})
export class TripFormComponent {
	readonly range = new FormGroup({
		start: new FormControl<Date | null>(null),
		end: new FormControl<Date | null>(null),
	}, { validators: this.dateRangeValidator });

	//ValidatorFn is what is used to validate a Form
	private dateRangeValidator(range: AbstractControl) {

		const startDate = range.get('start')?.value;
		const endDate = range.get('end')?.value;

		if (startDate && endDate) {
			if (endDate <= startDate)
				return { wrongDates: true};
		}

		return null;
	}

}
