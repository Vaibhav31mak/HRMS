  import { Component, OnInit } from '@angular/core';
  import { FormBuilder, FormGroup, Validators } from '@angular/forms';
  import { Router } from '@angular/router';
  import { ToastrService } from 'ngx-toastr';
  import { DeptServicesService } from 'src/app/services/dept-services.service';
  import { ReactiveFormsModule } from '@angular/forms'; // Import this
import { IDepartment } from 'src/app/interfaces/IDepartment';


  @Component({
    selector: 'app-add-department',
    templateUrl: './add-department.component.html',
    styleUrls: ['./add-department.component.css']
  })
  export class AddDepartmentComponent implements OnInit {
    departmentForm!: FormGroup;
    isSubmitted = false;

    constructor(
      private fb: FormBuilder,
      private deptService: DeptServicesService,
      private toastr: ToastrService,
      private router: Router
    ) {}

    ngOnInit(): void {
      this.departmentForm = this.fb.group({
        departmentName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z ]{3,30}$/)]],
      });
    }

    get f() {
      return this.departmentForm.controls;
    }

    onSubmit() {
      this.isSubmitted = true;
      if (this.departmentForm.invalid) {
        this.toastr.error('Please fill all fields correctly');
        return;
      }
    
      const departmentData: IDepartment = {
        name: this.departmentForm.value.departmentName,
      };
      console.log("here")
      this.deptService.createDepartment(departmentData).subscribe({
        next: () => {
          console.log("inside")
          this.toastr.success('Department added successfully!');
          this.router.navigate(['/department/display']);
          this.resetForm(); // Rename the method call to resetForm
        },
        error: (err) => {
          this.toastr.error('Failed to add department');
          console.error('Error:', err);
        }
      });
    }
    

    resetForm() {
      this.departmentForm.reset();
      this.isSubmitted = false;
    }
  }
