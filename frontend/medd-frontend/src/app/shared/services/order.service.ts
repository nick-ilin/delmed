import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrderRequest, Medicine, OrderResponse } from '../models/medicine.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = 'http://localhost:5002/api';

  constructor(private http: HttpClient) {}

  getMedicines(): Observable<Medicine[]> {
    return this.http.get<Medicine[]>(`${this.apiUrl}/medicine`);
  }

  createOrder(request: CreateOrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/order`, request);
  }

  getOrder(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/order/${id}`);
  }

  updateOrderStatus(orderId: number, status: number): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/order/${orderId}/status/${status}`, {});
  }
}
