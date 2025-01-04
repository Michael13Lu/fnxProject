import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Repository } from '../models/repository.model';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  private apiUrl = `${environment.apiUrl}/api/Repository`; 

  constructor(private http: HttpClient) { }

  // מתודה לחיפוש מאגרי מידע
  searchRepositories(query: string): Observable<Repository[]> {
    const headers = this.getAuthHeaders(); 
    return this.http
      .get<Repository[]>(`${this.apiUrl}/search?query=${query}`, { headers, withCredentials: true  })
      .pipe(
        catchError(this.handleError)
      );
  }

  // מתודה להוספת מאגר מידע לסימניות
  addBookmark(repository: any): Observable<any> {
    const headers = this.getAuthHeaders();
  
    console.log('שולח את המאגר לסימניות:', repository); 
  
    return this.http
      .post(`${this.apiUrl}/bookmark`, repository, { headers, withCredentials: true  })
      .pipe(
        catchError((error) => {
          console.error('שגיאה בהוספת מאגר לסימניות:', error);
          return this.handleError(error); 
        }),
        tap((response) => {
          console.log('הסימניה נוספה בהצלחה:', response);
        })
      );
  }

  // מתודה לקבלת רשימת הסימניות
  getBookmarks(): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http
      .get(`${this.apiUrl}/bookmarks`, { headers, withCredentials: true  })
      .pipe(
        catchError(this.handleError)
      );
  }

  // מתודה להסרת סימניה
  removeBookmark(id: string): Observable<any> {
    const headers = this.getAuthHeaders(); 
    return this.http
      .delete(`${this.apiUrl}/bookmark/${id}`, { headers, withCredentials: true  })
      .pipe(
        catchError(this.handleError) 
      );
  }

  // מתודה לקבלת כותרות עם אסימון
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      console.error('לא נמצא אסימון JWT ב-localStorage');
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}` 
    });
  }

  // מתודה לטיפול בשגיאות
  private handleError(error: any): Observable<never> {
    console.error('אירעה שגיאה:', error); 
    return throwError(() => new Error(error.message || 'שגיאת שרת'));
  }
}
