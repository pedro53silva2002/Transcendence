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
import { TripMemberService } from '../../service/trip/Member';

interface FriendDto {
  friendshipId: number;
  userId: number;
  username: string;
  displayName: string;
  profilePhotoUrl: string | null;
}

@Component({
  selector: 'app-add-member-dialog',
  imports: [MatDialogModule, MatButtonModule, MatIconModule, NgOptimizedImage, TranslocoModule],
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
  //private readonly friendService = inject(FriendService)
  private readonly memberService = inject(TripMemberService);

  protected readonly friends = signal<FriendDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly adding = signal(false);

  readonly tripId = this.data.tripId;
  readonly alreadyAdded = this.data.alreadyAdded;

  ngOnInit(): void {
    // Add the friends service logic, getting all the friends.
    //Delete this when we have the friends logic
    const available = this.getMockedFriends().filter((f) => !this.alreadyAdded.includes(f.userId));
    this.friends.set(available);
    this.loading.set(false);
  }

  protected addMember(friend: FriendDto): void {
    if (this.adding()) return;

    if (this.tripId === null) {
      this.dialogRef.close({
        userId: friend.userId,
        displayName: friend.displayName,
        profilePicture: friend.profilePhotoUrl,
        role: 'MEMBER' as const,
      });
      return;
    }

    this.adding.set(true);
    this.memberService
      .create({
        userId: friend.userId,
        role: 'MEMBER',
        tripId: this.tripId,
      })
      .subscribe({
        next: (result) => {
          if (result.data) {
            this.dialogRef.close(result.data);
          }
        },
        complete: () => this.adding.set(false),
        error: () => this.adding.set(false),
      });
  }

  protected cancel(): void {
    this.dialogRef.close();
  }

  private getMockedFriends(): FriendDto[] {
    return [
      {
        friendshipId: 1,
        userId: 101,
        username: 'Balouta',
        profilePhotoUrl: null,
        displayName: 'Maria Balouta',
      },
      {
        friendshipId: 2,
        userId: 102,
        username: 'Miguelote',
        profilePhotoUrl: null,
        displayName: 'Miguel Hot',
      },
      {
        friendshipId: 3,
        userId: 103,
        username: 'Coletes',
        profilePhotoUrl: null,
        displayName: 'Coletis Sonecas',
      },
      {
        friendshipId: 4,
        userId: 104,
        username: 'dadiaz',
        profilePhotoUrl: null,
        displayName: 'Diogo',
      },
      {
        friendshipId: 5,
        userId: 105,
        username: 'Lalalala',
        profilePhotoUrl: null,
        displayName: 'Michale Jackson',
      },
      {
        friendshipId: 6,
        userId: 106,
        username: 'gomes',
        profilePhotoUrl: null,
        displayName: 'Coelho Mau',
      },
      {
        friendshipId: 7,
        userId: 107,
        username: 'gomes',
        profilePhotoUrl: null,
        displayName: 'Coelho Mau',
      },
      {
        friendshipId: 8,
        userId: 108,
        username: 'gomes',
        profilePhotoUrl: null,
        displayName: 'Coelho Mau',
      },
    ];
  }
}
