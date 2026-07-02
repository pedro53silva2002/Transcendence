import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { takeUntilDestroyed, toObservable, toSignal } from '@angular/core/rxjs-interop';
import { filter, map, of, switchMap } from 'rxjs';
import { StatCardsComponent } from "../../../shared/stat-cards/stat-cards.component";
import { MatAnchor } from "@angular/material/button";
import { UserService } from '../../logic/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { EditProfileComponent } from './edit/edit-profile/edit-profile.component';
import { SessionService } from '../../logic/services/session.service';
import { ActivatedRoute } from '@angular/router';
import { SearchParams } from '../../logic/services/search.service';
import { TripService } from '../../logic/services/trip.service';
import { AsyncPipe } from '@angular/common';
import { ProfileItineraryCardComponent } from './profile-itinerary-card/profile-itinerary-card.component';
import { VisitedCountryCardComponent } from "./visited-country-card/visited-country-card.component";
import { UserDto, UserOrderByFieldsDto, UserSearchFieldsDto } from '../../logic/dtos/user.dto';
import { getUserAvatarUrl } from '../../logic/utils/minio-url.util';
import { FriendshipService } from '../../logic/services/friendship.service';
import { FriendRequestService } from '../../logic/services/friend-request.service';
import { CreateFriendRequestDto, FriendRequestDto } from '../../logic/dtos/friend-request.dto';
import { ConfirmationPopUpComponent } from '../../../shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { CreateFriendshipDto } from '../../logic/dtos/friendship.dto';

type FriendshipState = 'own_profile' | 'not_friends' | 'pending_sent' | 'friends' | 'pending_received';

@Component({
	selector: 'app-profile',
	imports: [MatIcon, TranslocoModule, StatCardsComponent, MatAnchor, AsyncPipe, ProfileItineraryCardComponent, VisitedCountryCardComponent],
	templateUrl: './profile.component.html',
	styleUrl: './profile.component.scss',
})
export default class ProfileComponent {

	private authService = inject(SessionService);
	private dialog = inject(MatDialog);
	private route = inject(ActivatedRoute);
	private userService = inject(UserService);
	private readonly translocoService = inject(TranslocoService);
	private readonly tripService = inject(TripService);
	private readonly friendshipService = inject(FriendshipService);
	private readonly friendRequestService = inject(FriendRequestService);
	private readonly destroyRef = inject(DestroyRef);

	protected readonly getUserAvatarUrl = getUserAvatarUrl;

	//logged username
	protected readonly authUsername = computed(() => this.authService.user()?.username ?? null);
	//profile username
	protected readonly profileUsername = signal<string>("");
	//to manage the visitedUser information
	protected readonly visitedUser = signal<UserDto>({} as UserDto);
	//bool signal to check if the profile is from the logged user or not
	protected readonly isOwnProfile = computed(() => this.authUsername() === this.profileUsername());
	protected readonly areFriends = signal<boolean>(false);
	protected readonly isPendingRequestSent = signal<boolean>(false);
	protected readonly isPendingRequestReceived = signal<boolean>(false);

	protected readonly itineraries$ = toObservable(this.visitedUser).pipe(
		filter((user): user is UserDto => user !== null && user.id !== undefined),
		switchMap(user => this.tripService.searchTripsByUserId(user.id)),
		map(response => response.data)
	);

	//to evaluate the situation between the auth and the visited user
	protected readonly currentRelationState = computed<FriendshipState>(() => {

		if (this.isOwnProfile())
			return 'own_profile';
		if (this.isPendingRequestSent())
			return 'pending_sent';
		if (this.areFriends())
			return 'friends';
		if (this.isPendingRequestReceived())
			return 'pending_received';

		return 'not_friends';
	});

	//to fill the text of the profile button
	protected readonly buttonLabel = computed<string>(() => {
		const state = this.currentRelationState();

		const labels: Record<FriendshipState, string> = {
			own_profile: 'profile.edit-btn',
			not_friends: 'profile.add-friend-btn',
			pending_sent: 'profile.cancel-request-btn',
			pending_received: 'profile.accept-request-btn',
			friends: 'profile.remove-friend-btn'
		};

		return labels[state];
	});

	constructor() {
		//extract profileUsername from URL
		this.route.paramMap.pipe(takeUntilDestroyed()).subscribe(params => {
			const usernameFromUrl = params.get('username') ?? "";
			this.profileUsername.set(usernameFromUrl);

			this.loadProfileData(usernameFromUrl);

			//check the relation between auth user and visited user
			// if (!this.isOwnProfile())
			// 	this.checkFriendshipStatus(username);
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
			}
		});
	}

	private checkFriendshipStatus(username: string): void {
		this.friendshipService.friendshipExists(this.profileUsername()).subscribe((result) => {
			this.areFriends.set(result.data ?? false);
		});

		this.friendshipService

	}

	protected handleButtonFunction(): void {
		const state = this.currentRelationState();

		switch (state) {
			case 'own_profile':
				this.openEditProfile();
				break;
			case 'not_friends':
				this.addFriend();
				break;
			case 'pending_sent':
				this.cancelFriendRequest();
				break;
			case 'pending_received':
				this.acceptFriendRequest();
				break;
			case 'friends':
				this.removeFriend();
				break;
		}
	}

	removeFriend(): void {

		const translatedTitle = this.translocoService.translate('profile.delete-user-title');
		const translatedMessage = this.translocoService.translate('profile.delete-friend-message');

		const dialogRef = this.dialog.open(ConfirmationPopUpComponent, {
			data: {
				title: translatedTitle,
				message: translatedMessage
			}
		});

		dialogRef.afterClosed().pipe(
			//filters by accepting the mat dialog
			filter((userConfirmed) => userConfirmed === true),

			switchMap(() => this.friendshipService.delete(this.visitedUser().id)),
			takeUntilDestroyed(this.destroyRef)).subscribe({
				next: () => {
					this.areFriends.set(false);
				}
			});
	}

	addFriend(): void {

		const authId = this.authService.me()?.id;
		if (!authId)
			return;

		const dto: CreateFriendRequestDto = {
			senderId: authId,
			receiverId: this.visitedUser().id
		}

		this.friendRequestService.create(dto).subscribe((success) => {
			this.isPendingRequestSent.set(true);
		});
	}

	cancelFriendRequest(): void {

		const translatedTitle = this.translocoService.translate('profile.delete-user-title');
		const translatedMessage = this.translocoService.translate('profile.delete-friend-request-message');

		const dialogRef = this.dialog.open(ConfirmationPopUpComponent, {
			data: {
				title: translatedTitle,
				message: translatedMessage
			}
		});

		dialogRef.afterClosed().pipe(
			//filters by accepting the mat dialog
			filter((userConfirmed) => userConfirmed === true),

			switchMap(() => this.friendRequestService.delete(this.visitedUser().id)),
			takeUntilDestroyed(this.destroyRef)).subscribe({
				next: () => {
					this.isPendingRequestSent.set(false);
				}
			});
	}

	acceptFriendRequest(): void {

		const authId = this.authService.me()?.id;
		if (!authId)
			return;

		const dto: CreateFriendshipDto = {
			userId1: authId,
			userId2: this.visitedUser().id
		}

		this.friendshipService.create(dto).subscribe((success) => {
			this.isPendingRequestReceived.set(false);
			this.areFriends.set(true);
		});
	}


	openEditProfile(): void {
		//extracts the user info
		const userToEdit = this.visitedUser();

		//to send the user data to the mat-dialog
		const dialogRef = this.dialog.open(EditProfileComponent, {
			panelClass: 'custom-profile-dialog',
			data: { user: userToEdit }
		});

		dialogRef.afterClosed().subscribe((updatedUser: UserDto | undefined) => {
			if (updatedUser)
				this.visitedUser.set(updatedUser);
		})

	}
}
