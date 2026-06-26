import {
	ChangeDetectionStrategy,
	Component,
	computed,
	signal,
	inject,
	viewChild,
	output,
	effect,
	input,
} from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { filter, map, switchMap } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomScrollbarComponent } from '../../layout/custom-scrollbar/custom-scrollbar.component';
import { ItineraryService } from './services/itinerary.service';
import { TripService } from '../plan-a-trip/services/trip.service';
import { MatIcon } from '@angular/material/icon';
import { MatError } from '@angular/material/form-field';
import { TranslocoModule } from '@jsverse/transloco';
import { SessionService } from '../../logic/services/session.service';
import { ItineraryDto } from './dtos/itinerary.dto';

@Component({
	selector: 'app-itinerary',
	imports: [
		CustomScrollbarComponent,
		MatIcon,
		MatError,
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
	private readonly authService = inject(SessionService);

	protected totalPrice = signal<number>(0);
	public totalPriceChanged = output<number>(); //the channel to send the totalPrice to the main component

	public isAdmin = input.required<boolean>();

	protected loggedUserId = this.authService?.me()?.id;

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

	private readonly initialTotalPrice = toSignal(
		toObservable(this.tripId).pipe(
			filter((id) => id > 0),
			switchMap((id) => this.itineraryService.getTotalPrice(id)),
			map((res) => res.data?.totalPrice ?? 0)
		),
		{ initialValue: 0 }
	);

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
			.subscribe((res) => {
				this.items.set(res.data?.content ?? [])
			});

		effect(() => {
			this.totalPrice.set(this.initialTotalPrice());
		})

		effect(() => {
			this.totalPriceChanged.emit(this.totalPrice());
		});
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

					this.totalPrice.update(costSum => (costSum ?? 0) + (res.data?.expectedPrice ?? 0));
				}
			});
	}

	removeItem(id: number): void {

		//identify the item that is being removed
		const deletingItem = this.items().find(it => it.id === id);

		this.itineraryService
			.delete(id)
			.subscribe(() => {
				//removes the item
				this.items.update((list) => list.filter((it) => it.id !== id));

				//subtracts the removed item from the totalPrice
				if (deletingItem?.expectedPrice) {
					const newTotal = (this.totalPrice() ?? 0) - deletingItem.expectedPrice;
					if (newTotal <= 0)
						this.totalPrice.set(0);
					else
						this.totalPrice.update(costSum => (costSum ?? 0) - deletingItem.expectedPrice);
				}
			});
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
