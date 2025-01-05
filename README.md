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

---

## How to Run

### 1. Clone the Repository
```bash
git clone https://github.com/YOUR_USERNAME/YOUR_PROJECT_NAME.git
cd YOUR_PROJECT_NAME
```

### 2. Install Dependencies
For the Angular frontend:
```bash
cd frontend
npm install
```

For the .NET backend:
```bash
cd ../backend
dotnet restore
```

### 3. Set Up Environment Variables
- For the backend, create a `.env` file and configure your GitHub API credentials.
- For the frontend, update the `environment.ts` file with your API endpoint URL.

### 4. Run the Backend
```bash
cd backend
dotnet run
```
This will start the Web API server on `http://localhost:5000`.

### 5. Run the Frontend
```bash
cd ../frontend
ng serve
```
This will start the Angular application on `http://localhost:4200`.

### 6. Access the Application
- Open a browser and navigate to `http://localhost:4200`.
- Log in using the following credentials:
  - Username: `admin`
  - Password: `admin`

### 7. Build for Production
To build the Angular application for production:
```bash
cd frontend
ng build --prod
```

To publish the backend:
```bash
cd ../backend
dotnet publish -c Release
```

---

## Additional Notes
- Make sure you have the latest versions of Node.js, npm, Angular CLI, and .NET installed on your machine.
- For any issues, open a ticket in the GitHub repository.

