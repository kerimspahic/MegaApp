import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:5001';

  constructor(private http: HttpClient, private router: Router) {}

  login(userData: { UserName: string; Password: string }) {
    return this.http.post(`${this.apiUrl}/Auth/Login`, userData);
  }

  register(userData: {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
  }) {
    return this.http.post(`${this.apiUrl}/Auth/Register`, userData);
  }

  storeToken(token: string) {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
}
