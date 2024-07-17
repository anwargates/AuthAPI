import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalFormProductsComponent } from './modal-form-products.component';

describe('ModalFormProductsComponent', () => {
  let component: ModalFormProductsComponent;
  let fixture: ComponentFixture<ModalFormProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ModalFormProductsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModalFormProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
