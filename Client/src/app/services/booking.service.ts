import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum TransportationType {
    Bus = 0,
    Plane = 1,
}

export interface BookingDto {
  offerId: string;
  numberOfPeople: number;
  transportationType: TransportationType;
}

export interface Booking {
  id: string;
  offerId: string;
  numberOfPeople: number;
  totalPrice: number;
  status: BookingStatus;
}

export enum BookingStatus {
  Pending = 0,
  Confirmed = 1,
  Cancelled = 2,
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7089/api/Booking'; 

  createBooking(bookingData: BookingDto): Observable<Booking> {
    return this.http.post<Booking>(`${this.apiUrl}`, bookingData);
  }

  getBooking(bookingId: string): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/${bookingId}`);
  }

  getUserBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/my-bookings`);
  }

  cancelBooking(bookingId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${bookingId}/cancel`, {});
  }
}