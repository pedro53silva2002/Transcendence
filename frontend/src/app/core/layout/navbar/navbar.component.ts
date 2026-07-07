import { AsyncPipe } from '@angular/common';
import {
	ChangeDetectorRef,
	Component,
	computed,
	HostListener,
	inject,
	input,
	OnInit,
	signal,
	ViewChild,
	ChangeDetectionStrategy,
} from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormField } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import {
	catchError,
	debounceTime,
	distinctUntilChanged,
	from,
	map,
	Observable,
	of,
	switchMap,
} from 'rxjs';
import { LoginComponent } from '../../feature/auth/login/login.component';
import { RegisterComponent } from '../register/register.component';
import { TranslocoModule } from '@jsverse/transloco';
import { LanguageButtonComponent } from '../../../shared/language-button/language-button.component';
import { UserService } from '../../logic/services/user.service';
import { SessionService } from '../../logic/services/session.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AuthService as OtherAuth } from '../../logic/services/auth.service';
import { FriendRequestService } from '../../logic/services/friend-request.service';
import { UserDto } from '../../logic/dtos/user.dto';
import { getUserAvatarUrl } from '../../../core/logic/utils/minio-url.util';

@Component({
	selector: 'app-navbar',
	standalone: true,
	imports: [
		MatButtonModule,
		MatMenuModule,
		ReactiveFormsModule,
		MatFormField,
		MatInputModule,
		MatAutocompleteModule,
		RouterLink,
		RouterLinkActive,
		MatIconModule,
		MatSidenavModule,
		AsyncPipe,
		TranslocoModule,
		LanguageButtonComponent,
	],
	templateUrl: './navbar.component.html',
	styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
	showHome = input(false);
	showProfile = input(false);
	showPlanATrip = input(false);
	showMyTrips = input(false);
	showFriends = input(false);
	showMyExpenses = input(false);
	showSearch = input(false);
	showLanguageSelector = input(false);
	showLogout = input(false);
	showLogin = input(false);
	showRegister = input(false);

	private readonly userService = inject(UserService);
	private readonly authService = inject(SessionService);
	private readonly router = inject(Router);
	private readonly otherAuth = inject(OtherAuth);
	private readonly dialog = inject(MatDialog);
	private readonly cd = inject(ChangeDetectorRef);

	protected readonly getUserAvatarUrl = getUserAvatarUrl;

	public isDashboardRoute = false;

	public authUsername = computed(() => this.authService.user()?.username ?? null);
	private readonly friendRequestService = inject(FriendRequestService);

	readonly currentUserId = computed(() => this.authService.me()?.id);
	readonly sentRequests = signal<Set<number>>(new Set());

	ngOnInit(): void {
		if (!this.authService.getOAuthResult()) {
			if (sessionStorage.getItem('auth_origin') === 'register') {
				this.openRegister();
			} else if (sessionStorage.getItem('auth_origin') === 'login') {
				this.openLogin();
			}
		}
		this.isDashboardRoute = this.router.url.includes('home');
	}

	openLogin(): void {
		const loginPopup = this.dialog.open(LoginComponent, {
			panelClass: 'custom-login-dialog'
		});

		//to remove the google error message when we close and open the component again
		loginPopup.afterClosed().subscribe(() => {
			this.authService.setOAuthSuccess(true);
		});
	}

	openRegister(): void {
		const registerPopup = this.dialog.open(RegisterComponent, {});

		//to remove the google error message when we close and open the component again
		registerPopup.afterClosed().subscribe(() => {
			this.authService.setOAuthSuccess(true);
		});
	}

	searchControl = new FormControl('');

	filteredUsers: Observable<UserDto[]> = this.searchControl.valueChanges.pipe(
		debounceTime(300),
		distinctUntilChanged(),
		switchMap((text) => {
			if (!text || text.trim() === '') return of([]);
			return from(
				this.userService.search({
					search: { username: { op: 'STARTSWITH', value: text } },
					orderBy: [{ field: 'username', descending: false }],
					pageSize: 10,
				}),
			).pipe(
				map((res) => {
					const users = res.data?.content ?? [];
					const me = this.authUsername();

					//to prevent the me profile to appear in the search
					return users.filter(user => user.username !== me);
				}),
				catchError((err) => {
					console.error('User search failed:', err);
					return of([]);
				}),
			);
		}),
	);

  sendFriendRequest(user: UserDto, event: Event): void {
    event.stopPropagation();
    const myId = this.currentUserId();
    if (!myId) return;

    this.friendRequestService
      .create({ senderId: myId, receiverId: user.id })
      .subscribe({
        next: () => this.sentRequests.update((s) => new Set([...s, user.id])),
        error: () => this.sentRequests.update((s) => new Set([...s, user.id])),
      });
  }

  logout(): void {
    this.otherAuth.logout();
    this.router.navigate(['/']);
  }

	displayUser(user: UserDto | string | null): string {
		if (!user || typeof user === 'string') return user ?? '';
		return user.displayName;
	}

	selectUser(user: UserDto): void {
		this.router.navigate(['/profile', user.id]);
	}

	goToProfile(event: MatAutocompleteSelectedEvent): void {
		const selectedUser = event.option.value as UserDto;

		if (selectedUser && selectedUser.username) {
			this.router.navigate(['/profile', selectedUser.username]);
			//reset to clean the selected state
			this.searchControl.setValue(null, { emitEvent: false });
			//to clean the input focus in the panel and close it
			event.option.deselect();
		}
	}

	//to prevent the previous search from appearing again when we click a second time in the search users field
	clearOnFocus() {
		this.searchControl.setValue('', { emitEvent: true });
	}

	//to close the side menu when the screen is resized to desktop size (if it's open)
	@ViewChild(MatMenuTrigger) menuTrigger!: MatMenuTrigger;

	@HostListener('window:resize', ['$event'])
	onResize(event: any) {
		if (window.innerWidth > 938 && this.menuTrigger && this.menuTrigger.menuOpen) {
			this.menuTrigger.closeMenu();
		}
	}
}
