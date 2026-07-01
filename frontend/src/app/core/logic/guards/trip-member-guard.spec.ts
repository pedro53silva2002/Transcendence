import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { tripMemberGuard } from './trip-member-guard';

describe('tripMemberGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => tripMemberGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
