import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { forkJoin, of, switchMap } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TripService } from '../plan-a-trip/services/trip.service';
import { TripMemberService } from '../service/trip/member.service';
import { TripDto } from '../plan-a-trip/dtos/trip.dto';
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
  private readonly memberService = inject(TripMemberService);

  readonly trips = signal<TripDto[]>([]);
  readonly memberCounts = signal<Record<number, number>>({});
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
  readonly displayTrips = computed(() => {
    const id = this.upcomingTripId();
    const list = this.trips();
    if (id == null) return list;
    const upcoming = list.filter((trip) => trip.id === id);
    const rest = list.filter((trip) => trip.id !== id);
    return [...upcoming, ...rest];
  });

  ngOnInit(): void {
    this.tripService
      .search({ pageSize: 99999, orderBy: [{ field: 'startDate', descending: false }] })
      .pipe(
        switchMap((response) => {
          const tripList = response.data?.content ?? [];
          if (tripList.length === 0) {
            return of({ tripList, counts: {} as Record<number, number> });
          }
          const memberRequests = tripList.map((trip) =>
            this.memberService.search({ pageSize: 99999 }, trip.id).pipe(
              map((r) => ({ tripId: trip.id, count: r.data?.content.length ?? 0 })),
              catchError(() => of({ tripId: trip.id, count: 0 })),
            ),
          );
          return forkJoin(memberRequests).pipe(
            map((results) => ({
              tripList,
              counts: Object.fromEntries(results.map((r) => [r.tripId, r.count])) as Record<number, number>,
            })),
          );
        }),
      )
      .subscribe(({ tripList, counts }) => {
        this.trips.set(tripList);
        this.memberCounts.set(counts);
        this.isLoading.set(false);
      });
  }

  getMemberCount(tripId: number): number {
    return this.memberCounts()[tripId] ?? 0;
  }
}
