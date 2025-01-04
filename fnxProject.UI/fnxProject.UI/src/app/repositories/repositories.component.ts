import { Component, OnInit } from '@angular/core';
import { RepositoryService } from './services/repository.service';
import { Repository } from './models/repository.model';

@Component({
  selector: 'app-repositories',
  templateUrl: './repositories.component.html',
  styleUrls: ['./repositories.component.css']
})
export class RepositoriesComponent implements OnInit {
  repositories: Repository[] = [];
  bookmarks: Repository[] = [];
  errorMessage: string = '';
  searchQuery: string = ''; 
  isLoading: boolean = false;
  activeSection: string = 'gallery';

  constructor(private repositoryService: RepositoryService) {}

  ngOnInit(): void {
    this.loadBookmarks();
  }

  // מעבר בין החלקים
  showSection(section: string): void {
    this.activeSection = section;
  }

  // חיפוש מאגרי מידע
  searchRepositories(): void {
    if (!this.searchQuery.trim()) {
      this.errorMessage = 'Please enter a repository name.';
      return;
    }

    this.isLoading = true;
    this.repositoryService.searchRepositories(this.searchQuery).subscribe({
      next: (data) => {
        this.repositories = data;
        this.errorMessage = this.repositories.length ? '' : 'No repositories found.';
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = `Error searching repositories: ${err.message}`;
        this.isLoading = false;
      }
    });
  }

  // הוספה/הסרה של סימניה
  bookmark(repository: Repository): void {
    repository.isBookmarked = !repository.isBookmarked;

    if (repository.isBookmarked) {
      this.bookmarks.push(repository); // Добавляем в закладки
    } else {
      this.removeBookmark(repository); // Удаляем из закладок
    }
  }

  // הסרת סימניה
  removeBookmark(repository: Repository): void {
    this.bookmarks = this.bookmarks.filter((b) => b.id !== repository.id);
  }
  // טעינת סימניות
  loadBookmarks(): void {
    this.repositoryService.getBookmarks().subscribe({
      next: (data) => {
        this.bookmarks = data;
      },
      error: (err) => {
        console.error('Error loading bookmarks:', err);
      }
    });
  }
}
