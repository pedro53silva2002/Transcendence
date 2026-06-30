import { Component, input } from '@angular/core';
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-visited-country-card',
  imports: [MatIconModule],
  templateUrl: './visited-country-card.component.html',
  styleUrl: './visited-country-card.component.scss',
})
export class VisitedCountryCardComponent {

	isOwnProfile = input.required<boolean>();
}
