import {
  ChangeDetectionStrategy,
  Component,
  computed,
  signal,
  inject,
  viewChild,
} from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { filter, map, switchMap } from 'rxjs';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { CustomScrollbarComponent } from '../../layout/custom-scrollbar/custom-scrollbar.component';
import { ItineraryService } from '../service/trip/itinerary.service';
import { ItineraryDto } from '../dtos/trip/itinerary.dto';
import { TripService } from '../plan-a-trip/services/trip.service';
import { MatIcon } from '@angular/material/icon';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-itinerary',
  imports: [NavbarComponent, CustomScrollbarComponent, MatIcon, TranslocoModule],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ItineraryComponent {
  private readonly itineraryService = inject(ItineraryService);
  private readonly tripService = inject(TripService);

  readonly tripId = toSignal(
    inject(ActivatedRoute).paramMap.pipe(map((p) => Number(p.get('tripId')))),
    { initialValue: 0 },
  );

  // Signals can't do async work directly. toObservable() lets us pipe the signal
  // through switchMap (cancels the previous HTTP call if tripId changes mid-flight),
  // then toSignal() converts the result back so computed() and the template can read it.
  private readonly trip = toSignal(
    toObservable(this.tripId).pipe(
      filter((id) => id > 0),
      switchMap((id) => this.tripService.getById(id)),
      map((res) => res.data ?? null),
    ),
    { initialValue: null },
  );

  readonly maxDays = computed(() => {
    const trip = this.trip();
    if (!trip) return 1;
    const start = new Date(trip.startDate);
    const end = new Date(trip.endDate);
    return Math.max(1, Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24)));
  });

  readonly showForm = signal(false);
  readonly currentDay = signal(1);
  readonly items = signal<ItineraryDto[]>([]);
  readonly titleValue = signal('');
  readonly costValue = signal('');
  readonly infoValue = signal('');

  readonly canSave = computed(() => this.titleValue().trim().length > 0);
  readonly prevDisabled = computed(() => this.currentDay() <= 1);
  readonly nextDisabled = computed(() => this.currentDay() >= this.maxDays());

  readonly scrollbarRef = viewChild<CustomScrollbarComponent>('scrollbar');

  private readonly queryParams = computed(() => ({
    tripId: this.tripId(),
    day: this.currentDay(),
  }));

  // I needed to put this on constructor because if I do it onInit, it won't update.
  // I tried to work with ngOnInit, but did not get lucky with that.
  constructor() {
    toObservable(this.queryParams)
      .pipe(
        filter(({ tripId }) => tripId > 0),
        switchMap(({ tripId, day }) =>
          this.itineraryService.search({
            search: {
              tripId: { op: 'EQUAL', value: tripId },
              day: { op: 'EQUAL', value: day },
            },
            pageSize: 100,
          }),
        ),
      )
      .subscribe((res) => this.items.set(res.data?.content ?? []));
  }

  toggleForm(): void {
    this.showForm.update((v) => !v);
  }

  saveItem(): void {
    const title = this.titleValue().trim();
    if (!title) return;
    this.itineraryService
      .create({
        tripId: this.tripId(),
        title,
        description: this.infoValue().trim() || undefined,
        expectedPrice: Number(this.costValue()) || 0,
        day: this.currentDay(),
      })
      .subscribe((res) => {
        if (res.data) {
          this.items.update((list) => [...list, res.data!]);
          this.titleValue.set('');
          this.costValue.set('');
          this.infoValue.set('');
          setTimeout(() => {
            const vp = this.scrollbarRef()?.viewport().nativeElement;
            if (vp) vp.scrollTop = vp.scrollHeight;
          });
        }
      });
  }

  removeItem(id: number): void {
    this.itineraryService
      .delete(id)
      .subscribe(() => this.items.update((list) => list.filter((it) => it.id !== id)));
  }

  changeDay(delta: number): void {
    const next = this.currentDay() + delta;
    if (next < 1 || next > this.maxDays()) return;
    this.currentDay.set(next);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Enter') this.saveItem();
  }

  filterCost(event: Event): void {
    const input = event.target as HTMLInputElement;
    const filtered = input.value.replace(/[^\d.]/g, '');
    if (filtered !== input.value) input.value = filtered;
    this.costValue.set(filtered);
  }
}
