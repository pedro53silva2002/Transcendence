import { ChangeDetectionStrategy, Component, computed, signal, inject, viewChild, output, effect } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { catchError, filter, map, of, switchMap } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomScrollbarComponent } from '../../../shared/custom-scrollbar/custom-scrollbar.component';
import { ItineraryService } from '../../logic/services/itinerary.service';
import { TripService } from '../../logic/services/trip.service';
import { MatIcon } from '@angular/material/icon';
import { MatError } from '@angular/material/form-field';
import { TranslocoModule } from '@jsverse/transloco';
import { SessionService } from '../../logic/services/session.service';
import { ItineraryDto } from '../../logic/dtos/itinerary.dto';
import { TripDto } from '../../logic/dtos/trip.dto';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { getUserAvatarUrl } from '../../logic/utils/minio-url.util';
import { TripStateService } from '../../logic/services/trip-state.service';

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
	private readonly tripState = inject(TripStateService);

	protected readonly getUserAvatarUrl = getUserAvatarUrl;

	protected totalPrice = signal<number>(0);
	public totalPriceChanged = output<number>(); //the channel to send the totalPrice to the main component
	readonly showForm = signal(false);
	readonly currentDay = signal(1);
	protected loggedUserId = this.authService?.me()?.id;

	protected readonly dialogData = inject<{ itinerary: TripDto; showAddButton: boolean, profileRoute: boolean, showAvatar: boolean, showDeleteButton: boolean }>(
		MAT_DIALOG_DATA,
		{ optional: true }
	);

	public readonly members = computed(() => this.tripState.members());

	protected readonly isAdmin = computed(() => {
			const me = this.authService.me();
			if (!me)
				return false;
	
			const myMembership = this.members().find((m) => m.userId === me.id);
			return myMembership?.role === 'Admin';
		})

	readonly tripId = this.dialogData ? signal<number>(this.dialogData.itinerary.id) : toSignal(
		inject(ActivatedRoute).paramMap.pipe(map((p) => Number(p.get('id') || 0))),
		{ initialValue: 0 }
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

	readonly maxDays = computed (() => this.trip()?.duration ?? 1);

	readonly itemForm = this.formBuilder.nonNullable.group({
		title: ['', [Validators.required, Validators.required, Validators.maxLength(10)]],
		description: ['', [Validators.maxLength(15)]],
		expectedPrice: [null as unknown as number, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
	});

	readonly canSave = toSignal(this.itemForm.statusChanges.pipe(map((s) => s === 'VALID')), {
		initialValue: this.itemForm.valid,
	});
	readonly prevDisabled = computed(() => this.currentDay() <= 1);
	readonly nextDisabled = computed(() => this.currentDay() >= this.maxDays());

	readonly scrollbarRef = viewChild<CustomScrollbarComponent>('scrollbar');

	readonly allItems = signal<ItineraryDto[]>([]); //to store all itinerary items

	//to filter allItems by selected day
	readonly items = computed(() => 
		this.allItems().filter((item) => item.day == this.currentDay())
	);

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
		toObservable(this.tripId)
			.pipe(
				filter((id) => id > 0),
				switchMap((tripId) =>
					this.itineraryService.search({
						search: {
							tripId: { op: 'EQUAL', value: tripId },
						},
						pageSize: 100,
					}).pipe(
						catchError((err) => {
							console.error('Failed to load itinerary: ', err);
							return of(null); //to prevent the error from 'killing' the subscription
						})
					),
				),
			)
			.subscribe((res) => {
				if (res)
					this.allItems.set(res.data?.content ?? []);
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
					this.allItems.update((list) => [...list, res.data!]);
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
				this.allItems.update((list) => list.filter((it) => it.id !== id));

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
