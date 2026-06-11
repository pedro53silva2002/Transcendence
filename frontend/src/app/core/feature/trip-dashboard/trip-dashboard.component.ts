import { Component, inject, OnInit } from '@angular/core';
import { TripStateService } from '../plan-a-trip/services/trip-state.service';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { DatePipe } from '@angular/common';
import { TripService } from '../plan-a-trip/services/trip.service';
import { ConfirmationPopUpComponent } from '../../../shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { EMPTY, filter, switchMap } from 'rxjs';

@Component({
	selector: 'app-trip-dashboard',
	imports: [MatButtonModule,
		RouterLink,
		DatePipe,
		MatDialogModule,
		TranslocoModule],
	templateUrl: './trip-dashboard.component.html',
	styleUrl: './trip-dashboard.component.scss',
})
export class TripDashboardComponent {
	public tripStateService = inject(TripStateService);
	public tripService = inject(TripService);
	private dialog = inject(MatDialog);
	private translocoService = inject(TranslocoService);
	public route = inject(Router);

	deleteTrip(): void {

		const translatedTitle = this.translocoService.translate('trip-dashboard.delete-trip-title');
		const translatedMessage = this.translocoService.translate('trip-dashboard.delete-trip-message');

		const dialogRef = this.dialog.open(ConfirmationPopUpComponent, {
			data: {
				title: translatedTitle,
				message: translatedMessage
			}
		});

		dialogRef.afterClosed().pipe(
			filter(response => response === true),
			switchMap(() => {
				const tripId = this.tripStateService.trip()?.id;
				if (tripId) {
					return this.tripService.delete(tripId);
				}
				return EMPTY;
			})
		).subscribe({
			next: () => {
				this.route.navigate(['/home']);
			}
		});


	}
}
