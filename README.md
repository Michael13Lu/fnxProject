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

---

## How to Run

### 1. Clone the Repository
```bash
git clone https://github.com/Michael13Lu/fnxProject.git
cd fnxProject
```

### 2. Install Dependencies
For the Angular frontend:
```bash
cd fnxProject\fnxProject.UI\fnxProject.UI
npm install
```

For the .NET backend:
```bash
cd fnxProject\fnxProject.API\fnxProject.API\
dotnet restore
```

### 3. Set Up Environment Variables
- For the backend, create a `.env` file and configure your GitHub API credentials.
- For the frontend, update the `environment.ts` file with your API endpoint URL.

### 4. Run the Backend
```bash
cd fnxProject\fnxProject.API\fnxProject.API\
dotnet run
```
This will start the Web API server on `https://localhost:7151`.

### 5. Run the Frontend
```bash
cd fnxProject\fnxProject.UI\fnxProject.UI
ng serve
```
This will start the Angular application on `http://localhost:4200`.

### 6. Access the Application
- Open a browser and navigate to `http://localhost:4200`.
- Log in using the following credentials:
  - Username: `admin`
  - Password: `admin`

---

## Additional Notes
- For any issues, open a ticket in the GitHub repository.

