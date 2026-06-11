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
import { UserService } from '../../auth/services/user.service';
import { UserDto } from '../../auth/dtos/user.dto';

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
  private readonly memberService = inject(TripMemberService);
  private readonly userService = inject(UserService);

  protected readonly friends = signal<UserDto[]>([]);
  protected readonly loading = signal(false);
  protected readonly adding = signal(false);

  readonly tripId = this.data.tripId;
  readonly alreadyAdded = this.data.alreadyAdded;

  ngOnInit(): void {
    this.loading.set(true);
    this.userService
      .search({ pageSize: 50 })
      .subscribe({
        next: (result) => {
          if (result.data) {
            this.friends.set(
              result.data.content.filter((u) => !this.alreadyAdded.includes(u.id)),
            );
          }
        },
        complete: () => this.loading.set(false),
        error: () => this.loading.set(false),
      });
  }

  protected addMember(friend: UserDto): void {
    if (this.adding()) return;
    const tripId = this.tripId;

    if (tripId === null) {
      this.dialogRef.close({
        userId: friend.id,
        displayName: friend.displayName,
        profilePicture: friend.profilePhotoUrl,
        role: 'MEMBER' as const,
      });
      return;
    }

    this.adding.set(true);
    this.memberService
      .create({ tripId, userIds: [friend.id] }, tripId)
      .subscribe({
        next: (result) => {
          if (result.data) {
            this.dialogRef.close({
              userId: friend.id,
              displayName: friend.displayName,
              profilePicture: friend.profilePhotoUrl,
              role: 'MEMBER' as const,
            });
          }
        },
        complete: () => this.adding.set(false),
        error: () => this.adding.set(false),
      });
  }

  protected cancel(): void {
    this.dialogRef.close();
  }
}
