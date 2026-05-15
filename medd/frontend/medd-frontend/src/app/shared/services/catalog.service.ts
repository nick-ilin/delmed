import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { Medicine } from '../models/medicine.model';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private apiUrl = 'http://localhost:5001/api/medicine';

  // BehaviorSubject для хранения актуального списка
  private medicinesSubject = new BehaviorSubject<Medicine[]>([]);
  medicines$ = this.medicinesSubject.asObservable();

  constructor(private http: HttpClient) {}

  getMedicines(): Observable<Medicine[]> {
    return this.http.get<Medicine[]>(this.apiUrl);
  }

  createMedicine(data: any): Observable<Medicine> {
    return this.http.post<Medicine>(this.apiUrl, data).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('API Error:', error);
    return throwError(() => error);
  }

  updateMedicine(id: number, data: any): Observable<Medicine> {
    return this.http.put<Medicine>(`${this.apiUrl}/${id}`, data);
  }

  deleteMedicine(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
