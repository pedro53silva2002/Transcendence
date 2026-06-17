import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CloseButtonComponent } from '../../../../../shared/components/close-button/close-button.component';
import { TranslocoModule } from '@jsverse/transloco';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../../auth/services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
	selector: 'app-edit-profile',
	imports: [
			MatDialogModule,
			MatFormFieldModule,
			CloseButtonComponent, 
			TranslocoModule, 
			MatIconModule, 
			ReactiveFormsModule,
			MatButtonModule,
			MatInputModule,
		],
	templateUrl: './edit-profile.component.html',
	styleUrl: './edit-profile.component.scss',
})
export class EditProfileComponent implements OnInit {

	private authService = inject(AuthService);
	private formBuilder = inject(FormBuilder);
	private readonly destroyRef = inject(DestroyRef);

	protected readonly hideOld = signal(true);
	protected readonly hideNew = signal(true);
	protected readonly hideConfirm = signal(true);

	readonly editProfileForm = this.formBuilder.group({
		displayName: ['', [Validators.required, Validators.maxLength(25), Validators.minLength(3)]],
		description: ['', [Validators.maxLength(150)]],
		avatar: [null as string | File | null],
		password: [''], //adicionar uma validação de verificação se é a password atual do user? vale a pena?
		newPassword: ['', [this.passwordRulesValidator()]],
		confirmPassword: ['']
	}, {
		validators: [this.passwordsMatchValidator]
	});

	ngOnInit(): void {
		this.loadUserData();
	}

	private loadUserData(): void {

		this.authService.me().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
			next: (response) => {
				if (response && response.data) {
					const user = response.data;

					this.editProfileForm.patchValue({
						displayName: user.displayName,
						// description: user.description
					});
				}
			},
			error: (err) => {
				console.error('Erro ao carregar dados do utilizador:', err);
			}
		});
	}

	private passwordRulesValidator(): ValidatorFn {
		return (control: AbstractControl): ValidationErrors | null => {
			const newPassword = control.value ?? '';
			if (!newPassword)
				return null;

			const errors: ValidationErrors = {};
			if (newPassword.length < 8 || newPassword > 20)
				errors['length'] = true;
			if (!/[a-z]/.test(newPassword))
				errors['lowercase'] = true;
			if (!/[A-Z]/.test(newPassword))
				errors['uppercase'] = true;
			if (!/[0-9]/.test(newPassword))
				errors['digits'] = true;
			if (!/[^A-Za-z0-9]/.test(newPassword))
				errors['special'] = true;

			return Object.keys(errors).length ? errors : null;
		}
	}

	private passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
		const newPassword = group.get('newPassword')?.value;
		const confirmPassword = group.get('confirmPassword')?.value;

		if (newPassword && newPassword !== confirmPassword) {
			group.get('confirmPassword')?.setErrors({ passwordMismatch: true });
			return { passwordMismatch: true };
		}
		return null;
	}

	saveUpdate(): void {
		if (this.editProfileForm.invalid)
			return;

		console.log('Ready to update: ', this.editProfileForm.getRawValue());

		//enviar o UpdateUserDto para o backend
	}
}
