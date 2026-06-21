import { Component, inject, OnInit, signal } from '@angular/core';
import { TripStateService } from '../plan-a-trip/services/trip-state.service';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { DatePipe } from '@angular/common';
import { TripService } from '../plan-a-trip/services/trip.service';
import { ConfirmationPopUpComponent } from '../../../shared/pop-up/confirmation-pop-up/confirmation-pop-up.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { EMPTY, filter, switchMap } from 'rxjs';
import { MemberFormComponent } from '../member-form/member-form.component';
import ItineraryComponent from '../itinerary/itinerary.component';
import { SessionService } from '../../logic/services/session.service';

@Component({
	selector: 'app-trip-dashboard',
	imports: [MatButtonModule,
		RouterLink,
		DatePipe,
		MatDialogModule,
		TranslocoModule,
		MemberFormComponent,
		ItineraryComponent],
	templateUrl: './trip-dashboard.component.html',
	styleUrl: './trip-dashboard.component.scss',
})
export class TripDashboardComponent implements OnInit {
	public tripStateService = inject(TripStateService);
	public tripService = inject(TripService);
	private dialog = inject(MatDialog);
	private translocoService = inject(TranslocoService);
	public route = inject(Router);
	private activatedRoute = inject(ActivatedRoute);
	public authService = inject(SessionService);

	public isAdmin = signal(false);

	//Verify if state service is empty. If so, make the request to backend to fill the data.
	ngOnInit(): void {
		const idParam = this.activatedRoute.snapshot.paramMap.get('id');
		if (!idParam) return;
		const id = Number(idParam);

		if (this.tripStateService.trip()?.id === id) return;

		this.tripService.getById(id).subscribe({
			next: (response) => {
				if (response.data) this.tripStateService.setTrip(response.data);
			},
		});

		// if (this.authService.me()?.trips[id].role === 'ADMIN')
		// 	this.isAdmin.set(true); não está a funcionar...

		// console.log(this.isAdmin());
	}

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
