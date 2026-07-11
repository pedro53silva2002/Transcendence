import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitedCountryCardComponent } from './visited-country-card.component';

describe('VisitedCountryCardComponent', () => {
  let component: VisitedCountryCardComponent;
  let fixture: ComponentFixture<VisitedCountryCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisitedCountryCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(VisitedCountryCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
