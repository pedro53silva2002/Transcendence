import { ComponentFixture, TestBed } from '@angular/core/testing';

<<<<<<<< HEAD:frontend/src/app/shared/components/close-button/close-button.component.spec.ts
import { CloseButtonComponent } from './close-button.component';

describe('CloseButtonComponent', () => {
  let component: CloseButtonComponent;
  let fixture: ComponentFixture<CloseButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CloseButtonComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CloseButtonComponent);
========
import { AppHomeComponent } from './user-dashboard.component';

describe('AppHomeComponent', () => {
  let component: AppHomeComponent;
  let fixture: ComponentFixture<AppHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppHomeComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AppHomeComponent);
>>>>>>>> origin/feat/trips:frontend/src/app/core/layout/user-dashboard/user-dashboard.component.spec.ts
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
