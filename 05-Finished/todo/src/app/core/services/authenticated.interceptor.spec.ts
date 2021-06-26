import { TestBed } from '@angular/core/testing';

import { AuthenticatedInterceptor } from './authenticated.interceptor';

describe('AuthenticatedInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      AuthenticatedInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: AuthenticatedInterceptor = TestBed.inject(AuthenticatedInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
