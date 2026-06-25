import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { TranslocoModule } from '@jsverse/transloco';
import { AuthService } from '../auth/services/auth.service';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { map, of } from 'rxjs';
import { StatCardsComponent } from "../../../shared/stat-cards/stat-cards.component";
import { MatAnchor } from "@angular/material/button";
import { UserService } from '../auth/services/user.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EditProfileComponent } from './edit/edit-profile/edit-profile.component';
import { SessionService } from '../../logic/services/session.service';
import { ActivatedRoute } from '@angular/router';
import { UserDto, UserOrderByFieldsDto, UserSearchFieldsDto } from '../auth/dtos/user.dto';
import { SearchParams } from '../../logic/services/search.service';

@Component({
	selector: 'app-profile',
	imports: [MatIcon, TranslocoModule, StatCardsComponent, MatAnchor],
	templateUrl: './profile.component.html',
	styleUrl: './profile.component.scss',
})
export default class ProfileComponent {

	private authService = inject(SessionService);
	private dialog = inject(MatDialog);
	private route = inject(ActivatedRoute);
	private userService = inject(UserService);

	//logged username
	protected readonly authUsername = computed(() => this.authService.user()?.username ?? null);
	//profile username
	protected readonly profileUsername = signal<string | null>(null);
	//to manage the visitedUser information
	protected readonly visitedUser = signal<UserDto | null>(null);
	//bool signal to check if the profile is from the logged user or not
	protected readonly isOwnProfile = computed(() => this.authUsername() === this.profileUsername());

	protected readonly finalAvatarUrl = computed (() => {
		const photoUrl = this.visitedUser()?.profilePhotoUrl;

		if (!photoUrl)
			return null;

		//if it begins with these its google auth picture
		if (photoUrl.startsWith('http://') || photoUrl.startsWith('https://')) {
			return photoUrl.replace(/=s\d+(-c)?$/, '=s0');
		}

		//if not, we append the MINIO_ENDPOINT
		return `http://localhost:9000/${photoUrl}`;
	})

	constructor() {
		//extract profileUsername from URL
		this.route.paramMap.pipe(takeUntilDestroyed()).subscribe(params => {
			const usernameFromUrl = params.get('username');
			this.profileUsername.set(usernameFromUrl);

			this.loadProfileData(usernameFromUrl);
		})
	}

	private loadProfileData(username: string | null): void {
		if (!username)
			return;

		const searchParams: SearchParams<UserSearchFieldsDto, UserOrderByFieldsDto> = {
			search: {
				username: { op: 'EQUAL', value: username }
			},
			pageSize: 1
		};

		this.userService.search(searchParams).subscribe({
			next: (response) => {
				const users = response.data?.content ?? [];


				if (users.length > 0) {
					this.visitedUser.set(users[0]);
				}
				else
					this.visitedUser.set(null);
			}
		})
	}

	openEditProfile(): void {
		//extracts the user info
		const userToEdit = this.visitedUser();

		//to send the user data to the mat-dialog
		const dialogRef = this.dialog.open(EditProfileComponent, {
			data: { user: userToEdit }
		});

		dialogRef.afterClosed().subscribe((updatedUser: UserDto | undefined) => {
			if (updatedUser)
				this.visitedUser.set(updatedUser);
		})

	}
}
