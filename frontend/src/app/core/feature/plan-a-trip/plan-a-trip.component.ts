import { Component } from '@angular/core';
import { TripFormComponent } from "./trip-form/trip-form.component";
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-plan-a-trip',
  imports: [TripFormComponent, TranslocoModule, MatButtonModule],
  templateUrl: './plan-a-trip.component.html',
  styleUrl: './plan-a-trip.component.scss',
})
export class PlanATripComponent {}
