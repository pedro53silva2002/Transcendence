import { AsyncPipe } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  HostListener,
  inject,
  input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
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
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';
import { TranslocoModule } from '@jsverse/transloco';
import { LanguageButtonComponent } from '../../../shared/components/language-button/language-button.component';
import { UserService } from '../../feature/auth/services/user.service';
import { UserDto } from '../../feature/auth/dtos/user.dto';
import { SessionService } from '../../logic/services/session.service';
import { AuthService as OtherAuth } from '../../feature/auth/services/auth.service';

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

  constructor(
    private readonly dialog: MatDialog,
    private readonly cd: ChangeDetectorRef,
  ) {}

  private readonly userService = inject(UserService);
  private readonly authService = inject(SessionService);
  private readonly router = inject(Router);
  private readonly otherAuth = inject(OtherAuth);

  ngOnInit(): void {
    if (!this.authService.getOAuthResult()) {
      if (sessionStorage.getItem('auth_origin') === 'register') {
        this.openRegister();
      } else if (sessionStorage.getItem('auth_origin') === 'login') {
        this.openLogin();
      }
    }
  }

  openLogin(): void {
    const loginPopup = this.dialog.open(LoginComponent, {});

    //to remove the google error message when we close and open the component again
    loginPopup.afterClosed().subscribe(() => {
      this.authService.setOAuthSuccess(true);
    });
  }

  openRegister(): void {
    const registerPopup = this.dialog.open(RegisterComponent, {
      panelClass: 'register-dialog',
    });

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
        map((res) => res.data?.content ?? []),
        catchError((err) => {
          console.error('User search failed:', err);
          return of([]);
        }),
      );
    }),
  );

  async logout() {
    await this.otherAuth.logout();
    this.router.navigate(['/']);
  }

  displayUser(user: UserDto | string | null): string {
    if (!user || typeof user === 'string') return user ?? '';
    return user.displayName;
  }

  selectUser(user: UserDto): void {
    this.router.navigate(['/profile', user.id]);
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
