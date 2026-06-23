import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { FriendshipService } from '../service/social/friendship.service';
import { FriendRequestService } from '../service/social/friend-request.service';
import { SessionService } from '../../logic/services/session.service';
import { FriendshipDto } from '../dtos/social/friendship.dto';
import { FriendRequestDto } from '../dtos/social/friend-request.dto';
import { UserDto } from '../auth/dtos/user.dto';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

interface FriendEntry {
  friendshipId: number;
  friend: UserDto;
}

@Component({
  selector: 'app-friendship',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    NavbarComponent,
    FooterComponent,
    LoadingSpinnerComponent,
    ReactiveFormsModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './friendship.component.html',
  styleUrl: './friendship.component.scss',
})
export default class FriendshipComponent implements OnInit {
  private readonly friendshipService = inject(FriendshipService);
  private readonly friendRequestService = inject(FriendRequestService);
  private readonly sessionService = inject(SessionService);

  readonly isLoading = signal(true);
  readonly friendships = signal<FriendshipDto[]>([]);
  readonly pendingRequests = signal<FriendRequestDto[]>([]);
  readonly searchControl = new FormControl('');
  readonly searchTerm = signal('');
  readonly showRequests = signal(false);

  readonly pendingCount = computed(() =>
    this.pendingRequests().filter(r => r.status === 'Pending').length,
  );

  readonly filteredFriends = computed<FriendEntry[]>(() => {
    const term = this.searchTerm().toLowerCase().trim();
    const me = this.sessionService.me()?.username;
    return this.friendships()
      .map(f => ({
        friendshipId: f.id,
        friend: f.user1.username === me ? f.user2 : f.user1,
      }))
      .filter(
        ({ friend }) =>
          !term ||
          friend.username.toLowerCase().includes(term) ||
          friend.displayName.toLowerCase().includes(term),
      );
  });

  ngOnInit(): void {
    this.friendshipService.getAll().subscribe({
      next: res => {
        this.friendships.set(res.data ?? []);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });

    this.friendRequestService.getAll().subscribe({
      next: res => this.pendingRequests.set(res.data ?? []),
      error: () => {},
    });

    this.searchControl.valueChanges.subscribe(v => this.searchTerm.set(v ?? ''));
  }

  removeFriend(friendshipId: number): void {
    this.friendshipService.delete(friendshipId).subscribe(() => {
      this.friendships.update(list => list.filter(f => f.id !== friendshipId));
    });
  }
}
