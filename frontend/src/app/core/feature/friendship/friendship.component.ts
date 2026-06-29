import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { FriendshipService } from '../service/social/friendship.service';
import { FriendRequestService } from '../service/social/friend-request.service';
import { FriendDto } from '../dtos/social/friendship.dto';
import { FriendRequestDto } from '../dtos/social/friend-request.dto';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

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

  readonly isLoading = signal(true);
  readonly friends = signal<FriendDto[]>([]);
  readonly pendingRequests = signal<FriendRequestDto[]>([]);
  readonly searchControl = new FormControl('');
  readonly searchTerm = signal('');
  readonly showRequests = signal(false);

  readonly pendingRequests$ = computed(() =>
    this.pendingRequests().filter(r => r.status === 'Pending'),
  );

  readonly pendingCount = computed(() => this.pendingRequests$().length);

  readonly filteredFriends = computed<FriendDto[]>(() => {
    const term = this.searchTerm().toLowerCase().trim();
    return this.friends().filter(
      f => !term || f.username.toLowerCase().includes(term),
    );
  });

  ngOnInit(): void {
    this.friendshipService.getAll().subscribe({
      next: res => {
        this.friends.set(res.data ?? []);
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
      this.friends.update(list => list.filter(f => f.id !== friendshipId));
    });
  }

  acceptRequest(requestId: number): void {
    this.friendRequestService.accept(requestId).subscribe(() => {
      this.pendingRequests.update(list => list.filter(r => r.id !== requestId));
      this.friendshipService.getAll().subscribe({
        next: res => this.friends.set(res.data ?? []),
      });
    });
  }

  denyRequest(requestId: number): void {
    this.friendRequestService.delete(requestId).subscribe(() => {
      this.pendingRequests.update(list => list.filter(r => r.id !== requestId));
    });
  }
}
