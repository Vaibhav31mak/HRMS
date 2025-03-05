import { IDepartment } from 'src/app/interfaces/IDepartment';
import { DeptServicesService } from 'src/app/services/dept-services.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-department',
  templateUrl: './display-department.component.html',
  styleUrls: ['./display-department.component.css'],
})
export class DisplayDepartmentComponent implements OnInit {
  departments: IDepartment[] = [];
  modalRef?: BsModalRef;

  totalLength: any;
  page: number = 1;
  searchText: any;
  deleteDeptId: number = 1;

  constructor(
    private deptService: DeptServicesService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.deptService.getDepartments().subscribe((data) => {
      this.departments = data as IDepartment[];
      this.totalLength = this.departments.length;
    });
  }

  addDepartment() {
    this.router.navigate(['/department/add']);
  }

  deleteDepartment(id: number): void {
    this.modalRef?.hide();
    this.deptService.deleteDepartment(id).subscribe(() => {
      this.departments = this.departments.filter((dept) => dept.id !== id);
    });
  }

  updateDepartment(department: IDepartment) {
    this.router.navigate(['/department/update'], { state: { department } });
  }

  trackByFn(index: number, department: IDepartment) {
    return department.id;
  }

  decline(): void {
    this.modalRef?.hide();
  }

  openModal(template: TemplateRef<void>, id: number) {
    this.deleteDeptId = id;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
}
