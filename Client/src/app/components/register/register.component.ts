import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerData = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
  };

  successMessage = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  onRegister() {
    this.authService.register(this.registerData).subscribe({
      next: () => {
        this.successMessage = 'Registration successful! Please log in.';
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.errorMessage = 'Registration failed. Please try again.';
        console.error(err);
      },
    });
  }
}