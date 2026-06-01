import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanATripComponent } from './plan-a-trip.component';

describe('PlanATripComponent', () => {
  let component: PlanATripComponent;
  let fixture: ComponentFixture<PlanATripComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlanATripComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PlanATripComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
