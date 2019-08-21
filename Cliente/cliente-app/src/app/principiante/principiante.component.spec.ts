import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrincipianteComponent } from './principiante.component';

describe('PrincipianteComponent', () => {
  let component: PrincipianteComponent;
  let fixture: ComponentFixture<PrincipianteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrincipianteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrincipianteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
