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
import { getUserAvatarUrl } from '../../logic/utils/minio-url.util';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
	selector: 'app-member-form',
	imports: [
		MatButtonModule,
		MatIconModule,
		MatSelectModule,
		NgOptimizedImage,
		TranslocoModule,
		CustomScrollbarComponent,
		RouterLink,
		MatTooltipModule
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

	protected readonly getUserAvatarUrl = getUserAvatarUrl;

	readonly tripId = input<number | null>(null);
	readonly columns = input(2);
	readonly compact = input(false);
	readonly showRole = input(true);
	readonly showAddButton = input(true);
	readonly isDashboardRoute = signal(false);
	public readonly tripCreatorUserId = signal<number>(-1);

	// members are stored in TripStateService so the dashboard can read the same list
	public readonly members = computed(() => this.tripState.members());

	protected readonly adminCount = computed(() => 
		this.members().filter((m) => m.role === 'Admin').length);

	protected isSoleAdmin(member: TripMemberDto): boolean {
		return member.role === 'Admin' && this.adminCount() === 1;
	}

	protected readonly isAdmin = computed(() => {
		const me = this.sessionService.me();
		if (!me)
			return false;

		const myMembership = this.members().find((m) => m.userId === me.id);
		return myMembership?.role === 'Admin';
	})


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
					});
				}
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

	protected updateRole(
		memberId: number | undefined,
		userId: number,
		role: 'Admin' | 'Member',
	): void {

		if (!this.isAdmin())
			return; //to prevent the recently changed from admin to member to change again to admin

		if (role === 'Member') {
			const member = this.members().find((m) => m.userId === userId);
			if (member && this.isSoleAdmin(member))
				return; //to prevent from demoting the only admin
		}

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
}
