import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<{ token: string }> {
    const credentials = { name: username, password };
    return this.http.post<{ token: string }>(`${environment.apiUrl}/api/Auth/login`, credentials);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('jwtToken');
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
  }
}
