import { Component, inject, OnInit } from '@angular/core';
import { TripStateService } from '../plan-a-trip/services/trip-state.service';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-trip-dashboard',
  imports: [MatButtonModule, RouterLink],
  templateUrl: './trip-dashboard.component.html',
  styleUrl: './trip-dashboard.component.scss',
})
export class TripDashboardComponent {
	public tripStateService = inject(TripStateService);

}
