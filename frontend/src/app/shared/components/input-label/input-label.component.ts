import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ControlValueAccessor, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
	selector: 'app-input-label',
	imports: [MatFormFieldModule,
		MatInputModule,
		MatIconModule,
		ReactiveFormsModule
	],
	templateUrl: './input-label.component.html',
	styleUrl: './input-label.component.scss',
})
export class InputLabelComponent implements ControlValueAccessor {
	@Input() label: string = '';
	@Input() placeholder: string = '';
	@Input() icon?: string;
	@Input() type: 'text' | 'password' | 'email' | 'number' = 'text';
	@Input() control: FormControl = new FormControl();

	value: any = '';
	disabled = false;
	onChange: any = () => { };
	onTouched: any = () => { };

	writeValue(value: any): void {
		this.value = value;
	}

	registerOnChange(fn: any): void {
		this.onChange = fn;
	}

	registerOnTouched(fn: any): void {
		this.onTouched = fn;
	}

	setDisabledState?(isDisabled: boolean): void {
		this.disabled = isDisabled;
	}

	onInputChange(event: Event): void {
		const target = event.target as HTMLInputElement;
		this.value = target.value;
		this.onChange(this.value);
	  }
}
