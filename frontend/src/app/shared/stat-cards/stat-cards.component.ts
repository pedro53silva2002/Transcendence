import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { UserService } from '../../core/logic/services/user.service';
import { rxResource, toObservable, toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { SessionService } from '../../core/logic/services/session.service';
import { filter, map, of, switchMap } from 'rxjs';
import { VisitedCountryService } from '../../core/logic/services/visited-country.service';
import { VisitedCountryDto } from '../../core/logic/dtos/visited-countries.dto';

@Component({
	selector: 'app-stat-cards',
	imports: [TranslocoModule],
	templateUrl: './stat-cards.component.html',
	styleUrl: './stat-cards.component.scss',
})
export class StatCardsComponent {
	private readonly userService = inject(UserService);
	private readonly route = inject(Router);
	private readonly authService = inject(SessionService);
	private readonly visitedCountriesService = inject(VisitedCountryService);

	//to receive from the main component the userId
	public readonly id = input<number>();
	public readonly refreshTrigger = input<number>(0); //trigger to update the countries nr
	protected readonly visitedCountries = signal<number>(0);

	loggedUser = this.authService.me()?.id;

	protected readonly targetId = computed(() => {
		if (this.route.url.includes('home')) {
			return this.loggedUser;
		}
		return this.id();
	})

	//convert the http responde directly to a signal, avoiding subscribe
	protected readonly statCardsInfo = toSignal(
		toObservable(this.targetId).pipe(
			filter((id): id is number => id !== undefined && id !== null),
			switchMap((id) => this.userService.getUserStats(id)),
			map(response => response.data),
		)
	);

	constructor() {
		effect(() => {
			const userId = this.targetId();
			const _ = this.refreshTrigger(); //listens to the refresh trigger and updates the countries without refreshing the page

			if (userId) {
				this.visitedCountriesService.getCountVisitedCountries(userId).subscribe(response => {
					this.visitedCountries.set(response.data ?? 0);
				})
			}
		})
	}

	//function to handle the >99 situation
	private formatNumber(value: number | undefined | null): string {
		const num = value ?? 0;
		return num > 99 ? '+99' : num.toString();
	}

	//formats the signals
	protected readonly countriesCount = computed(() => this.formatNumber(this.visitedCountries()));
	protected readonly tripsCount = computed(() => this.formatNumber(this.statCardsInfo()?.tripsLeftThisYear));
	protected readonly daysCountdown = computed(() => {
		const days = this.statCardsInfo()?.daysUntilNextTrip;
		if (days === undefined || days === null || days < 0)
			return '0';
		return this.formatNumber(days);
	});
}
