import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AttendanceService } from 'src/app/services/attendance.service';
import { ToastrService } from 'ngx-toastr';
import { ReactiveFormsModule } from '@angular/forms'; // Import this module


@Component({
  selector: 'app-add-attendance-excel',
  templateUrl: './add-attendance-excel.component.html',
  styleUrls: ['./add-attendance-excel.component.css']
})
export class AddAttendanceExcelComponent implements OnInit {
  excelForm: FormGroup;
  selectedFile: File | null = null;

  ngOnInit(): void {
    this.excelForm = this.formBuilder.group({
      file: [null]
    });
  }
  
  @Output() close = new EventEmitter<void>(); // Emit event to close modal

  constructor(
    private formBuilder: FormBuilder,
    private attendanceService: AttendanceService,
    private toastr: ToastrService
  ) {
    this.excelForm = this.formBuilder.group({
      file: [null]
    });
  }

  onFileChange(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit() {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      this.attendanceService.addExcelAttendance(formData).subscribe({
        next: () => {
          this.toastr.success('File uploaded successfully');
          this.close.emit(); 
        },
        error: () => this.toastr.error('Error uploading file'),
        complete: () => console.log('Upload completed')
      });
    }
  }

  closeModal() {
    this.close.emit(); // Emit event to close modal
  }
}
