import { TestBed } from '@angular/core/testing';

import { QuizzRequestService } from './quizz-request.service';

describe('QuizzRequestService', () => {
  let service: QuizzRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QuizzRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
