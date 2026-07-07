import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileItineraryCardComponent } from './profile-itinerary-card.component';

describe('ProfileItineraryCardComponent', () => {
  let component: ProfileItineraryCardComponent;
  let fixture: ComponentFixture<ProfileItineraryCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileItineraryCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileItineraryCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
