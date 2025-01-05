# Project Specifications

You will create a GitHub repositories search page using their API.

## Technologies
- Angular 8+
- .NET Framework/.Net 6/8 (Web API)
- Bootstrap/Angular Material

## Requirements
1. **Search Functionality**  
   - The user will type the repository they would like to search.  
   - When searching (by pressing a button or using the Enter key), perform a request to:  
     `https://api.github.com/search/repositories?q=YOUR_SEARCH_KEYWORD` from the server side.

2. **Render Results**  
   - Display the results as gallery items.  
   - Each item should show:
     - Repository name
     - Avatar of the owner
     - A bookmark button

3. **Bookmark Repositories**  
   - When a user bookmarks a repository, store the entire result in the user's session (custom session implementation).

4. **Authentication**  
   - Use JWT for authentication between client/server.  
     **Credentials**:  
     - Username: `admin`  
     - Password: `admin`

5. **Bookmark Screen**  
   - Add a screen to display all bookmarked repositories.

6. **Upload Project**  
   - When the project is complete, upload it to GitHub.
