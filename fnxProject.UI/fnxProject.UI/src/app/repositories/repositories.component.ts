import { Component, OnInit } from '@angular/core';
import { RepositoryService } from './services/repository.service';
import { Repository } from './models/repository.model';

@Component({
  selector: 'app-repositories',
  templateUrl: './repositories.component.html',
  styleUrls: ['./repositories.component.css']
})
export class RepositoriesComponent implements OnInit{
  
  repositories: Repository[] = [];
  displayedColumns: string[] = ['name', 'ownerAvatar', 'bookmark'];
  query: string = 'test';

  constructor(private repositoryService: RepositoryService){}

  ngOnInit(): void {
    this.loadRepositories();
  }

  loadRepositories(): void {
    this.repositoryService.searchRepositories(this.query).subscribe({
      next: (data) => {
        this.repositories = data;
        console.log('Repositories loaded:', data);
      },
      error: (error) => {
        console.error('Error loading repositories:', error);
      },
      complete: () => {
        console.log('Repository loading complete.');
      },
    });
  }

  bookmark(repository: Repository): void {
    repository.isBookmarked = !repository.isBookmarked;
    console.log(`${repository.name} bookmarked:`, repository.isBookmarked);
  }
}
