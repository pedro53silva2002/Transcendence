import { NgOptimizedImage } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  OnInit,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { TranslocoModule } from '@jsverse/transloco';
import { MatDialog } from '@angular/material/dialog';
import { TripMemberService } from '../service/trip/Member';
import { AddMemberDialogComponent } from './add-member-dialog/add-member-dialog.component';
import { CustomScrollbarComponent } from '../../layout/custom-scrollbar/custom-scrollbar.component';

export interface MemberDisplayDto {
  userId: number;
  displayName: string;
  profilePicture: string | null;
  role: 'ADMIN' | 'MEMBER';
}

@Component({
  selector: 'app-member-form',
  imports: [MatButtonModule, MatIconModule, MatSelectModule, NgOptimizedImage, TranslocoModule, CustomScrollbarComponent],
  templateUrl: './member-form.component.html',
  styleUrl: './member-form.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberFormComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  private readonly memberService = inject(TripMemberService);

  readonly tripId = input<number | null>(null);

  readonly members = signal<MemberDisplayDto[]>([]);
  protected readonly loading = signal(false);

  ngOnInit(): void {
    const id = this.tripId();
    if (id === null) return;

    this.loading.set(true);
    this.memberService
      .search({
        search: { tripId: { op: 'EQUAL', value: id } },
        orderBy: [{ field: 'displayName', descending: false }],
        pageSize: 10,
      })
      .subscribe({
        next: (result) => {
          if (result.data) this.members.set(result.data);
        },
        complete: () => this.loading.set(false),
        error: () => this.loading.set(false),
      });
  }

  protected openAddMember(): void {
    const alreadyAdded = this.members().map((m) => m.userId);
    const ref = this.dialog.open(AddMemberDialogComponent, {
      data: { tripId: this.tripId(), alreadyAdded },
    });

    ref.afterClosed().subscribe((newMember: MemberDisplayDto | undefined) => {
      if (newMember) {
        this.members.update((list) => [...list, newMember]);
      }
    });
  }

  protected updateRole(userId: number, role: 'ADMIN' | 'MEMBER'): void {
    this.members.update((list) =>
      list.map((m) => (m.userId === userId ? { ...m, role } : m)),
    );
  }
}
