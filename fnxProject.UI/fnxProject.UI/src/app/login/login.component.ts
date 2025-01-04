import { Component } from '@angular/core';
import { LoginService } from './services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private loginService: LoginService, private router: Router) {}

    // מתודה לטיפול בטופס ההתחברות
    onSubmit(): void {
      this.loginService.login(this.username, this.password).subscribe({
        next: (response) => {
          localStorage.setItem('jwtToken', response.token);
          this.router.navigate(['/repositories']);
        },
        error: () => {
          this.errorMessage = 'Login error. Please check your username or password.';
        }
      });
    }
}
