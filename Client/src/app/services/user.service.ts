import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserLoginDto {
  email: string;
  password: string;
}

export interface UserRegisterDto {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7089/api/User'; 

  login(loginData: UserLoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginData);
  }

  register(registerData: UserRegisterDto): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/register`, registerData);
  }

  logout(): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/logout`, {});
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  removeToken(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}