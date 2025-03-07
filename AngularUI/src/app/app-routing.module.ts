import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './Components/landing/landing.component';
import { SignInComponent } from './Components/sign-in/sign-in.component';
import { OrganizationSettingsComponent } from './Components/organization-settings/organization-settings.component';
import { NewAdminComponent } from './Components/new-admin/new-admin.component';
import { AttendanceReportComponent } from './Components/attendance-report/attendance-report.component';
import { SalaryReportComponent } from './Components/salary-report/salary-report.component';
import { OfficalDaysComponent } from './Components/offical-days/offical-days.component';
import { AddEmployeeComponent } from './Components/add-employee/add-employee.component';
import { UpdateEmployeeComponent } from './Components/update-employee/update-employee.component';
import { DisplayEmployeeComponent } from './Components/display-employee/display-employee.component';
import { AddAttendanceComponent } from './Components/add-attendance/add-attendance.component';
import { AuthGuard } from './_guards/auth.guard';
import { preventLoginGuard } from './_guards/prevent-login.guard';
import { RolesComponent } from './Components/roles/roles.component';
import { UpdateRoleComponent } from './Components/update-role/update-role.component';
import { AddRoleComponent } from './Components/add-role/add-role.component';
import { SidebarComponent } from './Components/sidebar/sidebar.component';
import { UsersComponent } from './Components/users/users.component';
import { DisplayDepartmentComponent } from './Components/display-department/display-department.component';
import { AddDepartmentComponent } from './Components/add-department/add-department.component';
import { AddAttendanceExcelComponent } from './Components/add-attendance-excel/add-attendance-excel.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    component: SignInComponent,
    canActivate: [preventLoginGuard],
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'home', component: LandingComponent },
      {
        path: 'users',
        component: UsersComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'users/add',
        component: NewAdminComponent,
        canActivate: [AuthGuard],
      },
      { path: 'attendance/report', component: AttendanceReportComponent },
      { path: 'salary/report', component: SalaryReportComponent },
      { path: 'daysoff', component: OfficalDaysComponent },
      { path: 'employee/add', component: AddEmployeeComponent },
      { path: 'settings', component: OrganizationSettingsComponent },
      { path: 'employee/update', component: UpdateEmployeeComponent },
      { path: 'employee/display', component: DisplayEmployeeComponent },
      { path: 'attendance/add', component: AddAttendanceComponent },
      { path: 'roles', component: RolesComponent },
      { path: 'roles/update', component: UpdateRoleComponent },
      { path: 'roles/add', component: AddRoleComponent },
      {path: 'department/display', component: DisplayDepartmentComponent},
      {path: 'department/add', component: AddDepartmentComponent},
      {path: 'attendance/addExcel', component: AddAttendanceExcelComponent},
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
