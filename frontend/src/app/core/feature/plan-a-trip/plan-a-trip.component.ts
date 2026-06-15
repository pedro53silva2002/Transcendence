import { Component, inject, OnInit, signal, viewChild } from '@angular/core';
import { TripFormComponent } from './trip-form/trip-form.component';
import { TranslocoModule } from '@jsverse/transloco';
import { MatButtonModule } from '@angular/material/button';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { TripService } from './services/trip.service';
import { CreateTripDto, TripVisibility, UpdateTripDto } from './dtos/trip.dto';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { TripStateService } from './services/trip-state.service';
import { CountryDto } from './dtos/country.dto';
import { CityDto } from './dtos/city.dto';
import { DateTime } from 'luxon';
import { MemberFormComponent } from '../member-form/member-form.component';

@Component({
  selector: 'app-plan-a-trip',
  imports: [TripFormComponent, TranslocoModule, MatButtonModule, ReactiveFormsModule, RouterModule, MemberFormComponent],
  templateUrl: './plan-a-trip.component.html',
  styleUrl: './plan-a-trip.component.scss',
})
export class PlanATripComponent implements OnInit {
  tripForm = viewChild(TripFormComponent);
  memberForm = viewChild(MemberFormComponent);
  tripForm = viewChild(TripFormComponent);

  private readonly formBuilder = inject(FormBuilder);
  private readonly tripService = inject(TripService);
  private readonly router = inject(Router);
  private readonly tripStateService = inject(TripStateService);
  private readonly route = inject(ActivatedRoute);
  protected readonly currentTripId = signal<number | null>(null);

  public isEditMode = false;

  planATripForm = this.formBuilder.nonNullable.group({
    trip: this.formBuilder.nonNullable.group({
      tripName: ['', [Validators.required, Validators.maxLength(25), Validators.minLength(3)]],
      description: ['', [Validators.maxLength(250)]],
      country: [null as CountryDto | null, [Validators.required, PlanATripComponent.countryValidator]],
      city: [[] as CityDto[], [PlanATripComponent.cityValidator]],
      startDate: [null as any, [Validators.required]], // Alterado para aceitar o objeto Date inicial
      endDate: [null as any, [Validators.required]],
      budget: [null as unknown as number, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
      visibility: [TripVisibility.Private, [Validators.required]]
    }),
  });

  ngOnInit(): void {
    this.isEditMode = this.router.url.includes('edit');

    if (this.isEditMode) {
      const currentTrip = this.tripStateService.trip();
      const tripId = this.route.snapshot.paramMap.get('id');
      if (tripId) this.currentTripId.set(Number(tripId));

      if (currentTrip) {
        this.planATripForm.patchValue({
          trip: {
            tripName: currentTrip.tripName,
            description: currentTrip.description,
            country: currentTrip.country,
            city: currentTrip.city,
            startDate: currentTrip.startDate ? new Date(currentTrip.startDate) : null,
            endDate: currentTrip.endDate ? new Date(currentTrip.endDate) : null,
            budget: currentTrip.budget,
            visibility: currentTrip.visibility
          }
        });
      }
    }
  }

  submitTrip() {
    if (this.planATripForm.valid) {
      const formValue = this.planATripForm.getRawValue();

      if (!this.isEditMode) {
        if (formValue.trip.country) {
          const dto: CreateTripDto = {
            tripName: formValue.trip.tripName,
            description: formValue.trip.description || undefined,
            country: formValue.trip.country,
            city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
            startDate: this.formatToDateOnly(formValue.trip.startDate),
            endDate: this.formatToDateOnly(formValue.trip.endDate),
            budget: formValue.trip.budget,
            visibility: formValue.trip.visibility,
             members: {
              tripId: 0,
              userIds:
                this.memberForm()
                  ?.members()
                  .map((m) => m.userId) ?? [],
            },
          };

          this.tripService.create(dto).subscribe({
            next: (response) => {
              if (response.data?.id)
                this.currentTripId.set(response.data.id);
              if (response.data !== undefined)
                this.tripStateService.setTrip(response.data);
              this.router.navigate(['/trip-dashboard', response.data?.id]);
            },
            error: (error) => {
              console.error('Error creating trip:', error);
            }
          });
        }

      } else {
        const tripId = this.route.snapshot.paramMap.get('id');

        if (tripId && formValue.trip.country) {
          const updateDto: UpdateTripDto = {
            id: Number(tripId),
            tripName: formValue.trip.tripName,
            description: formValue.trip.description || undefined,
            country: formValue.trip.country,
            city: formValue.trip.city.length > 0 ? formValue.trip.city : undefined,
            startDate: this.formatToDateOnly(formValue.trip.startDate),
            endDate: this.formatToDateOnly(formValue.trip.endDate),
            budget: formValue.trip.budget,
            visibility: formValue.trip.visibility,
          };

          this.tripService.update(updateDto.id, updateDto).subscribe({
            next: (response) => {
              if (response.data !== undefined)
                this.tripStateService.setTrip(response.data);
              this.router.navigate(['/trip-dashboard', response.data?.id]);
            },
            error: (error) => {
              console.error('Error updating trip:', error);
            }
          });
        }
      }
    }
  }

  static countryValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;
    if (typeof value === 'string')
      return { countryNotSelected: true };
    if (typeof value === 'object' && !value.id)
      return { countryNotSelected: true };
    return null;
  }

  static cityValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value || (Array.isArray(value) && value.length === 0) || (typeof value === 'string' && value.trim() === '')) {
      return null;
    }
    if (typeof value === 'string') return { cityNotSelected: true };
    return null;
  }

  /**
   * Garante o formato estrito "YYYY-MM-DD" para o System.DateOnly do .NET
   */
  formatToDateOnly(date: any): string {
    if (!date) return '';

    if (typeof date === 'string') {
      return date.substring(0, 10);
    }

    if (date && typeof date.toISODate === 'function') {
      return date.toISODate()!;
    }

    if (date instanceof Date) {
      // Tratamento seguro para extrair a componente local YYYY-MM-DD do objeto Date
      const offset = date.getTimezoneOffset();
      const localDate = new Date(date.getTime() - (offset * 60 * 1000));
      return localDate.toISOString().substring(0, 10);
    }

    return '';
  }
}
