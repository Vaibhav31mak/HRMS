import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDepartment } from '../interfaces/IDepartment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DeptServicesService {
  private baseUrl = `${environment.baseUrl}/Department`;

  constructor(private http: HttpClient) {}

  // Get all departments
  getDepartments(): Observable<IDepartment[]> {
    return this.http.get<IDepartment[]>(this.baseUrl);
  }

  // Get department by name
  getDepartmentByName(name: string): Observable<IDepartment> {
    return this.http.get<IDepartment>(`${this.baseUrl}/${name}`);
  }

  // Create a new department
  // Create a new department
createDepartment(department: IDepartment): Observable<any> {
  return this.http.post(this.baseUrl, department, { responseType: 'text' }).pipe(
    catchError((error: HttpErrorResponse) => {
      console.error('Server Error:', error);

      if (error.status === 200 && !error.ok) {
        console.error('Unexpected Response Format:', error.error);
        return throwError(() => new Error('Invalid server response format.'));
      }

      return throwError(() => new Error('There was a problem with the server. Please try again later.'));
    })
  );
}


  // Update an existing department
  updateDepartment(id: number, department: IDepartment): Observable<string> {
    return this.http.put<string>(`${this.baseUrl}/${id}`, department).pipe(
      catchError(this.handleError)
    );
  }

  // Delete a department
  // Delete a department
deleteDepartment(id: number): Observable<any> {
  return this.http.delete(`${this.baseUrl}/${id}`, { responseType: 'text' }).pipe(
    catchError((error: HttpErrorResponse) => {
      console.error('Server Error:', error);

      if (error.status === 200 && !error.ok) {
        console.error('Unexpected Response Format:', error.error);
        return throwError(() => new Error('Invalid server response format.'));
      }

      return throwError(() => new Error('There was a problem with the server. Please try again later.'));
    })
  );
}


  // Handle HTTP errors
  private handleError(error: HttpErrorResponse) {
    console.error('Server Error:', error);
    return throwError(() => new Error('There was a problem with the server. Please try again later.'));
  }
}