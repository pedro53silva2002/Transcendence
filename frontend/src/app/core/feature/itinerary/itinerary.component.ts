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
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { CustomScrollbarComponent } from '../../layout/custom-scrollbar/custom-scrollbar.component';
import { ItineraryService } from '../service/trip/itinerary.service';
import { ItineraryDto } from '../dtos/trip/itinerary.dto';
import { TripService } from '../plan-a-trip/services/trip.service';
import { MatIcon } from '@angular/material/icon';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-itinerary',
  imports: [
    CustomScrollbarComponent,
    MatIcon,
    MatError,
    MatFormField,
    MatLabel,
    MatInput,
    TranslocoModule,
    ReactiveFormsModule,
  ],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ItineraryComponent {
  private readonly itineraryService = inject(ItineraryService);
  private readonly tripService = inject(TripService);
  private readonly formBuilder = inject(FormBuilder);

  readonly tripId = toSignal(
    inject(ActivatedRoute).paramMap.pipe(map((p) => Number(p.get('id')))),
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

  readonly itemForm = this.formBuilder.nonNullable.group({
    title: ['', [Validators.required, Validators.required, Validators.maxLength(20)]],
    description: ['', [Validators.maxLength(30)]],
    expectedPrice: [null as unknown as number, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
  });

  readonly showForm = signal(false);
  readonly currentDay = signal(1);
  readonly items = signal<ItineraryDto[]>([]);

  readonly canSave = toSignal(this.itemForm.statusChanges.pipe(map((s) => s === 'VALID')), {
    initialValue: this.itemForm.valid,
  });
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
    if (this.itemForm.invalid) return;
    const { title, description, expectedPrice } = this.itemForm.getRawValue();
    this.itineraryService
      .create({
        tripId: this.tripId(),
        title: title.trim(),
        description: description.trim() || undefined,
        expectedPrice,
        day: this.currentDay(),
      })
      .subscribe((res) => {
        if (res.data) {
          this.items.update((list) => [...list, res.data!]);
          this.itemForm.reset();
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
}
