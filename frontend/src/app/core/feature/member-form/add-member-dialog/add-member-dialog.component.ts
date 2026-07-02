import { NgOptimizedImage } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { TranslocoModule } from '@jsverse/transloco';
import { TripMemberService } from '../services/member.service';
import { FriendDto } from '../../dtos/social/friendship.dto';
import { TripMemberDto } from '../../itinerary/dtos/member.dto';
import { CloseButtonComponent } from '../../../../shared/components/close-button/close-button.component';
import { FriendshipService } from '../../service/social/friendship.service';

@Component({
  selector: 'app-add-member-dialog',
  imports: [MatDialogModule, MatButtonModule, MatIconModule, NgOptimizedImage, TranslocoModule, CloseButtonComponent],
  templateUrl: './add-member-dialog.component.html',
  styleUrl: './add-member-dialog.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddMemberDialogComponent implements OnInit {
  private readonly dialogRef = inject(MatDialogRef<AddMemberDialogComponent>);
  private readonly data = inject<{ tripId: number | null; alreadyAdded: number[] }>(
    MAT_DIALOG_DATA,
  );
  private readonly memberService = inject(TripMemberService);
  private readonly friendshipService = inject(FriendshipService);

  protected readonly friends = signal<FriendDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly adding = signal(false);

  readonly tripId = this.data.tripId;
  readonly alreadyAdded = this.data.alreadyAdded;

  ngOnInit(): void {
    this.loading.set(true);
    this.friendshipService.getAll().subscribe({
      next: (result) => {
        if (result.data) {
          this.friends.set(
            result.data.filter((f) => !this.alreadyAdded.includes(f.friendId)),
          );
        }
      },
      complete: () => this.loading.set(false),
      error: () => this.loading.set(false),
    });
  }

  protected addMember(friend: FriendDto): void {
    if (this.adding()) return;
    const tripId = this.tripId;

    // No trip yet (create flow): return a pending member; id/tripId/timestamps fill in once saved.
    if (tripId === null) {
      const pending: TripMemberDto = {
        userId: friend.friendId,
        displayName: friend.username,
        profilePicture: friend.profilePhotoUrl,
        role: 'Member',
      };
      this.dialogRef.close(pending);
      return;
    }

    this.adding.set(true);
    this.memberService.create({ tripId, userIds: [friend.friendId] }, tripId).subscribe({
      next: (result) => {
        if (result.data?.[0]) this.dialogRef.close(result.data[0]);
      },
      complete: () => this.adding.set(false),
      error: () => this.adding.set(false),
    });
  }

  protected cancel(): void {
    this.dialogRef.close();
  }
}
