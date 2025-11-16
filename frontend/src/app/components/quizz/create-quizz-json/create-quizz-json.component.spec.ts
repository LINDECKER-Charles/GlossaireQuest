import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateQuizzJsonComponent } from './create-quizz-json.component';

describe('CreateQuizzJsonComponent', () => {
  let component: CreateQuizzJsonComponent;
  let fixture: ComponentFixture<CreateQuizzJsonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateQuizzJsonComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateQuizzJsonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
