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
import { TripMemberService } from '../service/trip/member.service';
import { AddMemberDialogComponent } from './add-member-dialog/add-member-dialog.component';
import { CustomScrollbarComponent } from '../../layout/custom-scrollbar/custom-scrollbar.component';
import { TripStateService } from '../plan-a-trip/services/trip-state.service';
import { TripMemberDto } from '../dtos/trip/member.dto';

@Component({
  selector: 'app-member-form',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    NgOptimizedImage,
    TranslocoModule,
    CustomScrollbarComponent,
  ],
  templateUrl: './member-form.component.html',
  styleUrl: './member-form.component.scss',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberFormComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  private readonly memberService = inject(TripMemberService);
  private readonly tripState = inject(TripStateService);

  readonly tripId = input<number | null>(null);

  // members are stored in TripStateService so the dashboard can read the same list
  public readonly members = this.tripState.members;
  protected readonly loading = signal(false);

  ngOnInit(): void {
    const id = this.tripId();
    // create flow: no trip yet, start with an empty shared list
    if (id === null) {
      this.tripState.setMembers([]);
      return;
    }

    this.loading.set(true);
    this.memberService
      .search(
        {
          search: { tripId: { op: 'EQUAL', value: id } },
          orderBy: [{ field: 'userId', descending: false }],
          pageSize: 10,
        },
        id,
      )
      .subscribe({
        next: (result) => {
          if (result.data) this.tripState.setMembers(result.data.content);
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

    ref.afterClosed().subscribe((newMember: TripMemberDto | undefined) => {
      if (newMember) {
        this.tripState.addMember(newMember);
      }
    });
  }

  protected updateRole(userId: number, role: 'Admin' | 'Member'): void {
    this.tripState.updateMemberRole(userId, role);
  }
}
