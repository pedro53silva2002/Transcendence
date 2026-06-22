import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { NgOptimizedImage, DatePipe, UpperCasePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { TripDto } from '../../plan-a-trip/dtos/trip.dto';
import { UtripBubbleComponent } from '../utrip-bubble/utrip-bubble.component';

@Component({
  selector: 'app-trip-card',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage, DatePipe, UpperCasePipe, MatIconModule, RouterLink, UtripBubbleComponent],
  templateUrl: './trip-card.component.html',
  styleUrl: './trip-card.component.scss',
})
export class TripCardComponent {
  trip = input.required<TripDto>();
  memberCount = input<number>(0);

  readonly destination = computed(() => {
    const t = this.trip();
    if (t.city && t.city.length > 0) return t.city[0].name;
    return t.country.name;
  });

  readonly imagePath = computed(() => getSeasonImage(this.trip().startDate));

  readonly isUpcoming = computed(() => {
    const start = new Date(this.trip().startDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    start.setHours(0, 0, 0, 0);
    return start > today;
  });
}

function getSeasonImage(startDate: string): string {
  const month = new Date(startDate).getMonth() + 1;
  if (month >= 3 && month <= 5) return '/images/spring.png';
  if (month >= 6 && month <= 8) return '/images/summer.png';
  if (month >= 9 && month <= 11) return '/images/autumn.png';
  return '/images/winter.png';
}
