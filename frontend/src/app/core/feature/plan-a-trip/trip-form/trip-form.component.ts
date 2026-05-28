import { Component, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from '@angular/material/input';
import { TranslocoModule } from '@jsverse/transloco';

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
})
export class TripFormComponent {
	readonly range = new FormGroup({
		start: new FormControl<Date | null>(null),
		end: new FormControl<Date | null>(null),
	});
}
