import {
	ChangeDetectionStrategy,
	Component,
	computed,
	inject,
	input,
	OnInit,
	signal,
} from '@angular/core';
import { DatePipe, UpperCasePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { UtripBubbleComponent } from '../../core/feature/my-trips/utrip-bubble/utrip-bubble.component';
import { TripService } from '../../core/logic/services/trip.service';
import { TripDto } from '../../core/logic/dtos/trip.dto';
import { MatDialog } from '@angular/material/dialog';

@Component({
	selector: 'app-trip-card',
	changeDetection: ChangeDetectionStrategy.OnPush,
	imports: [DatePipe, UpperCasePipe, MatIconModule, RouterLink, UtripBubbleComponent],
	templateUrl: './trip-card.component.html',
	styleUrl: './trip-card.component.scss',
})
export class TripCardComponent implements OnInit {

	private readonly tripService = inject(TripService);
	private readonly router = inject(Router);
	private readonly dialog = inject(MatDialog);

	trip = input.required<TripDto>();
	/* Set by the parent; only one card in the list is the upcoming trip. */
	isUpcoming = input<boolean>(false);
	readonly memberCount = signal<number>(0);


	readonly destination = computed(() => {
		const t = this.trip();
		if (t.city && t.city.length > 0) return t.city[0].name;
		return t.country.name;
	});

	readonly imagePath = computed(() => getSeasonImage(this.trip().startDate));

	ngOnInit(): void {

		this.tripService
			.getById(this.trip().id)
			.pipe(
				map((response) => response.data?.members?.length ?? 0),
				catchError(() => of(0)),
			)
			.subscribe((count) => this.memberCount.set(count));
	}
}

function getSeasonImage(startDate: string): string {
	const month = new Date(startDate).getMonth() + 1;
	if (month >= 3 && month <= 5) return '/images/spring.png';
	if (month >= 6 && month <= 8) return '/images/summer.png';
	if (month >= 9 && month <= 11) return '/images/autumn.png';
	return '/images/winter.png';
}
