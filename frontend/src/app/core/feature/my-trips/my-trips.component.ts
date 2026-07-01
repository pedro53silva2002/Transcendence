import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { forkJoin } from 'rxjs';
import { TripService } from '../../logic/services/trip.service';
import { TripDto } from '../../logic/dtos/trip.dto';
import { SessionService } from '../../logic/services/session.service';
import { TripCardComponent } from './trip-card/trip-card.component';

@Component({
  selector: 'app-my-trips',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoModule, TripCardComponent],
  templateUrl: './my-trips.component.html',
  styleUrl: './my-trips.component.scss',
})
export class MyTripsComponent implements OnInit {
  private readonly tripService = inject(TripService);
  private readonly sessionService = inject(SessionService);

  readonly trips = signal<TripDto[]>([]);
  readonly isLoading = signal(true);

  /* The single upcoming trip: the next one that hasn't started yet (earliest
     start date that is today or later). The list is sorted by startDate ascending. */
  readonly upcomingTripId = computed(() => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const upcoming = this.trips().find((trip) => {
      const start = new Date(trip.startDate);
      start.setHours(0, 0, 0, 0);
      return start >= today;
    });
    return upcoming?.id ?? null;
  });

  /* Trips to display: the upcoming trip is moved to the front, the rest keep
     their original (startDate ascending) order. */

  ngOnInit(): void {
    const tripIds = this.sessionService.me()?.trips?.map((t) => t.tripId) ?? [];

    if (tripIds.length === 0) {
      this.isLoading.set(false);
      return;
    }

    forkJoin(tripIds.map((id) => this.tripService.getById(id))).subscribe((responses) => {
      const trips = responses
        .map((r) => r.data)
        .filter((t): t is TripDto => t != null)
        .sort((a, b) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime());
      this.trips.set(trips);
      this.isLoading.set(false);
    });
  }
  readonly displayTrips = computed(() => {
    const id = this.upcomingTripId();
    const list = this.trips();
    if (id == null) return list;
    const upcoming = list.filter((trip) => trip.id === id);
    const rest = list.filter((trip) => trip.id !== id);
    return [...upcoming, ...rest];
  });
}
