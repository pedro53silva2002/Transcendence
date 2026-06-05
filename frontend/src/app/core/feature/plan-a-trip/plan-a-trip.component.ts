import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MemberFormComponent } from '../member-form/member-form.component';

@Component({
  selector: 'app-plan-a-trip',
  imports: [MemberFormComponent],
  templateUrl: './plan-a-trip.component.html',
  styleUrl: './plan-a-trip.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlanATripComponent {}
