import { Component, inject, input, output } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";
import { CountryDto } from '../../../logic/dtos/country.dto';
import { VisitedCountryService } from '../../../logic/services/visited-country.service';

@Component({
  selector: 'app-visited-country-card',
  imports: [MatIconModule],
  templateUrl: './visited-country-card.component.html',
  styleUrl: './visited-country-card.component.scss',
})
export class VisitedCountryCardComponent {

	private readonly visitedCountryService = inject(VisitedCountryService);

	public readonly isOwnProfile = input.required<boolean>();
	public readonly country = input.required<CountryDto>();
	public readonly countryRemoved = output<void>();

	removeCountry(countryId: number): void {
		this.visitedCountryService.removeCountry(countryId).subscribe({
			next: () => {
				this.countryRemoved.emit();
			}
		});
	}
}
