// Importing Modules
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';

// Importing Components
import { AppComponent } from './app.component';
import { SignInComponent } from './Components/sign-in/sign-in.component';
import { LandingComponent } from './Components/landing/landing.component';

import { NewAdminComponent } from './Components/new-admin/new-admin.component';
import { AttendanceReportComponent } from './Components/attendance-report/attendance-report.component';
import { SalaryReportComponent } from './Components/salary-report/salary-report.component';
import { OfficalDaysComponent } from './Components/offical-days/offical-days.component';
import { AddEmployeeComponent } from './Components/add-employee/add-employee.component';
import { DisplayEmployeeComponent } from './Components/display-employee/display-employee.component';
import { UpdateEmployeeComponent } from './Components/update-employee/update-employee.component';
import { OrganizationSettingsComponent } from './Components/organization-settings/organization-settings.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FilterPipe } from './pipes/filter.pipe';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { AddAttendanceComponent } from './Components/add-attendance/add-attendance.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptorInterceptor } from './_interceptors/loading-interceptor.interceptor';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { RolesComponent } from './Components/roles/roles.component';
import { UpdateRoleComponent } from './Components/update-role/update-role.component';
import { AddRoleComponent } from './Components/add-role/add-role.component';
import { SidebarComponent } from './Components/sidebar/sidebar.component';
import { UsersComponent } from './Components/users/users.component';
import { AddDepartmentComponent } from './Components/add-department/add-department.component';
import { DisplayDepartmentComponent } from './Components/display-department/display-department.component';
import { AddAttendanceExcelComponent } from './Components/add-attendance-excel/add-attendance-excel.component';

@NgModule({
  declarations: [
    AppComponent,
    SignInComponent,
    LandingComponent,
    NewAdminComponent,
    AttendanceReportComponent,
    SalaryReportComponent,
    OfficalDaysComponent,
    AddEmployeeComponent,
    DisplayEmployeeComponent,
    DisplayDepartmentComponent,
    UpdateEmployeeComponent,
    OrganizationSettingsComponent,
    FilterPipe,
    AddAttendanceComponent,
    AddAttendanceExcelComponent,
    AddDepartmentComponent,
    RolesComponent,
    UpdateRoleComponent,
    AddRoleComponent,
    SidebarComponent,
    UsersComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPaginationModule,
    BrowserAnimationsModule,

    ToastrModule.forRoot({
      timeOut: 3000, // Toast disappears after 3 seconds
      progressBar: true,
      positionClass: 'toast-bottom-right',
    }),
    NgxSpinnerModule.forRoot({
      type: 'line-scale-party',
    }),
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptorInterceptor,
      multi: true,
    },
  ],

  bootstrap: [AppComponent],
})
export class AppModule {}
