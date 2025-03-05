import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAttendanceExcelComponent } from './add-attendance-excel.component';

describe('AddAttendanceExcelComponent', () => {
  let component: AddAttendanceExcelComponent;
  let fixture: ComponentFixture<AddAttendanceExcelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddAttendanceExcelComponent]
    });
    fixture = TestBed.createComponent(AddAttendanceExcelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
