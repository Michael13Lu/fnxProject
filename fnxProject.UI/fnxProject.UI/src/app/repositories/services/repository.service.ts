import {HttpClient} from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Repository } from '../models/repository.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }

  searchRepositories(query: string): Observable<Repository[]> {
    return this.http.get<Repository[]>(`${environment.apiUrl}/api/Repository/search?query=${query}`);
  }
}
