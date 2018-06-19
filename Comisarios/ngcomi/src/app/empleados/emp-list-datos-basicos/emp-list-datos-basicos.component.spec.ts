import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpListDatosBasicosComponent } from './emp-list-datos-basicos.component';

describe('EmpListDatosBasicosComponent', () => {
  let component: EmpListDatosBasicosComponent;
  let fixture: ComponentFixture<EmpListDatosBasicosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpListDatosBasicosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpListDatosBasicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
