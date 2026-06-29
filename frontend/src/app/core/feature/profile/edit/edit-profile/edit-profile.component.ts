import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CloseButtonComponent } from '../../../../../shared/close-button/close-button.component';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../../auth/services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { ErrorStateMatcher } from '@angular/material/core';
import { ConfirmationPopUpComponent } from '../../../../../shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { EMPTY, filter, of, switchMap } from 'rxjs';
import { Router } from '@angular/router';
import { UserService } from '../../../auth/services/user.service';
import { UpdateUserDto, UserDto } from '../../../auth/dtos/user.dto';
import { SessionService } from '../../../../logic/services/session.service';

//shows the errors in real time
export class InstantErrorStateMatcher implements ErrorStateMatcher {
	isErrorState(control: AbstractControl | null): boolean {
		return !!(control && control.invalid && (control.dirty || control.touched));
	}
}

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
export class EditProfileComponent {

	//to receive the data transfered from the profile component
	private userData = inject<{ user: UserDto }>(MAT_DIALOG_DATA);
	//to send data back to the profile component
	private dialogRef = inject(MatDialogRef<EditProfileComponent>);

	private authService = inject(SessionService);
	private formBuilder = inject(FormBuilder);
	private readonly destroyRef = inject(DestroyRef);
	private translocoService = inject(TranslocoService);
	private dialog = inject(MatDialog);
	public router = inject(Router);
	public userService = inject(UserService);

	protected readonly matcher = new InstantErrorStateMatcher();

	protected readonly passwordVisible = signal(false);
	protected readonly confirmPasswordVisible = signal(false);
	protected readonly avatarPreview = signal<string | null>(null);
	protected readonly isGoogleAccount = signal(false);

	readonly editProfileForm = this.formBuilder.group({
		displayName: ['', [Validators.required, Validators.maxLength(15), Validators.minLength(3)]],
		description: ['', [Validators.maxLength(150)]],
		avatar: [null as string | File | null],
		password: [''],
		newPassword: ['', [this.passwordRulesValidator()]],
		confirmPassword: ['']
	}, {
		validators: [this.passwordsMatchValidator]
	});

	//this computed signal can read directly the form values as signals
	protected readonly passwordValue = computed(() => this.editProfileForm.getRawValue().newPassword ?? '');

	protected readonly passwordRules = computed(() => {
		const password = this.passwordValue();
		if (!password)
			return null;
		return {
			length: password.length >= 8 && password.length <= 20,
			lowercase: /[a-z]/.test(password),
			uppercase: /[A-Z]/.test(password),
			digits: /[0-9]/.test(password),
			special: /[^A-Za-z0-9]/.test(password)
		}
	})

	constructor() {
		this.loadUserData();
	}

	private loadUserData(): void {

		const user = this.userData?.user;

		if (user) {
			this.editProfileForm.patchValue({
				displayName: user.displayName,
				description: user.bio,
				avatar: user.profilePhotoUrl
			});

			if (user.profilePhotoUrl) {
				const isGooglePhoto = user.profilePhotoUrl.startsWith('http://') || user.profilePhotoUrl.startsWith('https://');
			
				if (isGooglePhoto) {
					this.avatarPreview.set(user.profilePhotoUrl.replace(/=s\d+(-c)?$/, '=s0'));
				} else {
					this.avatarPreview.set(`http://localhost:9000/${user.profilePhotoUrl}`);
				}
			}

			console.log(user.oAuthProvider);

			if (user.oAuthProvider === 'google')
				this.isGoogleAccount.set(true);
		}
	}

	private passwordRulesValidator(): ValidatorFn {
		return (control: AbstractControl): ValidationErrors | null => {
			const newPassword = control.value ?? '';
			if (newPassword === '')
				return null;

			const errors: ValidationErrors = {};
			if (newPassword.length < 8 || newPassword.length > 20)
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
		const newPassword = group.get('newPassword')?.value ?? '';
		const confirmPassword = group.get('confirmPassword')?.value ?? '';
		const confirmControl = group.get('confirmPassword');

		if (!newPassword && !confirmPassword) {
			if (confirmControl?.hasError('passwordMismatch')) {
				confirmControl.setErrors(null);
			}
			return null;
		}

		if (newPassword && newPassword !== confirmPassword) {
			confirmControl?.setErrors({ passwordMismatch: true });
			return { passwordMismatch: true };
		} else {
			if (confirmControl?.hasError('passwordMismatch')) {
				confirmControl.setErrors(null);
			}
		}
		return null;
	}

	saveUpdate(): void {

		if (this.editProfileForm.invalid)
			return;

		const user = this.userData?.user;

		const avatarValue = this.editProfileForm.value.avatar;

		const updateDto: UpdateUserDto = {
			email: user.email,
			username: user.username,
			displayName: this.editProfileForm.getRawValue().displayName ?? user.displayName,
			password: this.editProfileForm.getRawValue().password ? this.editProfileForm.getRawValue().password : null,
			bio: this.editProfileForm.getRawValue().description ?? user.bio,
			profilePhotoUrl: avatarValue instanceof File ? avatarValue : null
		}

		this.userService.update(updateDto).subscribe(response => {
			const updatedUser: any = response && 'data' in response ? response.data : response;
			
			this.authService.loadMe().subscribe();
			this.dialogRef.close(updatedUser);
		});
	}

	deleteAccount(): void {
		const translatedTitle = this.translocoService.translate('profile.delete-user-title');
		const translatedMessage = this.translocoService.translate('profile.delete-user-message');

		const dialogRef = this.dialog.open(ConfirmationPopUpComponent, {
			data: {
				title: translatedTitle,
				message: translatedMessage
			}
		});

		dialogRef.afterClosed().pipe(switchMap(result => {
			if (result === true)
				return this.userService.delete();
			return of(null);
		}), takeUntilDestroyed(this.destroyRef)).subscribe({
			next: (response) => {
				if (response) {
					this.dialogRef.close();
					this.authService.clearSession();
					this.router.navigate(['/']);
				}
			}
		});
	}

	//to open the window for file selection
	searchFile(inputHtml: HTMLInputElement): void {
		inputHtml.click();
	}

	//when a user selects an image
	onFileSelected(event: Event): void {
		const element = event.target as HTMLInputElement;
		const fileList: FileList | null = element.files;

		if (fileList && fileList.length > 0) {
			const file = fileList[0];
			const avatarControl = this.editProfileForm.controls.avatar;

			if (avatarControl) {
				avatarControl.patchValue(file);
				avatarControl.markAsDirty();
				avatarControl.updateValueAndValidity();
			}

			this.avatarPreview.set(URL.createObjectURL(file));
			element.value = '';
		}
	}
}