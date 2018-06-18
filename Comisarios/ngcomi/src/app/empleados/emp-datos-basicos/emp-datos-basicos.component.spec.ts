import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpDatosBasicosComponent } from './emp-datos-basicos.component';

describe('EmpDatosBasicosComponent', () => {
  let component: EmpDatosBasicosComponent;
  let fixture: ComponentFixture<EmpDatosBasicosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpDatosBasicosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpDatosBasicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
