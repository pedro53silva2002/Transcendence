import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoogleAuthButtonComponent } from './google-auth-button.component';

describe('GoogleAuthButtonComponent', () => {
  let component: GoogleAuthButtonComponent;
  let fixture: ComponentFixture<GoogleAuthButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GoogleAuthButtonComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(GoogleAuthButtonComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
