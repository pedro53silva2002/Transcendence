import { NgOptimizedImage } from '@angular/common';
import {
	ChangeDetectionStrategy,
	Component,
	computed,
	effect,
	inject,
	input,
	signal,
	ViewEncapsulation,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { TranslocoModule } from '@jsverse/transloco';
import { MatDialog } from '@angular/material/dialog';
import { AddMemberDialogComponent } from './add-member-dialog/add-member-dialog.component';
import { CustomScrollbarComponent } from '../../../shared/custom-scrollbar/custom-scrollbar.component';
import { TripStateService } from '../../logic/services/trip-state.service';
import { TripMemberDto } from '../../logic/dtos/member.dto';
import { Router, RouterLink } from '@angular/router';
import { SessionService } from '../../logic/services/session.service';
import { TripMemberService } from '../../logic/services/member.service';

@Component({
	selector: 'app-member-form',
	imports: [
		MatButtonModule,
		MatIconModule,
		MatSelectModule,
		NgOptimizedImage,
		TranslocoModule,
		CustomScrollbarComponent,
		RouterLink
	],
	templateUrl: './member-form.component.html',
	styleUrl: './member-form.component.scss',
	encapsulation: ViewEncapsulation.None,
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberFormComponent {
	private readonly dialog = inject(MatDialog);
	private readonly memberService = inject(TripMemberService);
	private readonly tripState = inject(TripStateService);
	private readonly route = inject(Router);
	private readonly sessionService = inject(SessionService);

	readonly tripId = input<number | null>(null);
	readonly columns = input(2);
	readonly compact = input(false);
	readonly showRole = input(true);
	readonly showAddButton = input(true);
	readonly isDashboardRoute = signal(false);
	public readonly tripCreatorUserId = signal<number>(-1);

	// members are stored in TripStateService so the dashboard can read the same list
	public readonly members = computed(() => this.tripState.members());
	protected readonly loading = signal(false);

	public isAdmin = input<boolean>();

	constructor() {

		if (this.route.url.includes('trip-dashboard'))
			this.isDashboardRoute.set(true);

		// Only fetch when tripId is a real id; skip null
		effect(() => {
			const id = this.tripId();

			// For the create flow (no tripId yet), seed the list with the creator as Admin.
			if (id === null) {
				const me = this.sessionService.me();
				if (me) {
					this.tripState.setMembers([{
						userId: me.id,
						username: me.username,
						displayName: me.displayName,
						profilePicture: me.profilePhotoUrl,
						role: 'Admin',
					}]);
					this.tripCreatorUserId.set(me.id);
				} else {
					this.tripState.setMembers([]);
				}
			}
			else {
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

							if (this.tripState.trip()?.createdBy)
								this.tripCreatorUserId.set(this.tripState.trip()?.createdBy ?? -1);
						},
						complete: () => this.loading.set(false),
						error: () => this.loading.set(false),
					});
				}
			}, { allowSignalWrites: true });
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

	protected updateRole(
		memberId: number | undefined,
		userId: number,
		role: 'Admin' | 'Member',
	): void {
		this.tripState.updateMemberRole(userId, role);

		const tripId = this.tripId();
		if (tripId !== null && memberId !== undefined) {
			this.memberService.update(memberId, { userId, role }, tripId).subscribe();
		}
	}

	protected removeMember(memberId: number | undefined, userId: number): void {
		const tripId = this.tripId();
		if (tripId !== null && memberId !== undefined) {
			this.memberService.delete(memberId, tripId).subscribe();
		}
		this.tripState.removeMember(userId);
	}

	protected canDeleteMember(memberUserId: number): boolean {
		if (!this.isAdmin())
			return false;

		return memberUserId !== this.tripCreatorUserId();
	}

	protected getAvatarUrl(photoUrl: string | null): string {
		if (!photoUrl)
			return 'images/default-avatar.png';

		if (photoUrl.startsWith('http://') || photoUrl.startsWith('https://')) {
			return photoUrl.replace(/=s\d+(-c)?$/, '=s0');
		}

		// Se for o caminho relativo do teu MinIO local
		return `http://localhost:9000/${photoUrl}`;
	}
}
