# AI Resume Career Platform

AI Resume Career Platform is a full-stack application designed to help users build stronger resumes, analyze job descriptions, match resumes with job opportunities, and receive AI-powered career recommendations.

## Current Features

- User registration
- Request validation with FluentValidation
- Password hashing
- Clean Architecture
- Repository Pattern
- Dependency Injection
- Entity Framework Core
- SQL Server integration
- Database migrations
- REST API testing

## Planned Features

- User login
- JWT authentication
- Role-based authorization
- Resume upload
- Resume parsing
- AI resume analysis
- Job-description matching
- ATS score calculation
- Career recommendations
- Interview preparation
- React frontend

## Technology Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation

### Frontend

- React
- TypeScript
- Vite

## Architecture

The backend follows Clean Architecture:

```text
API
  ↓
Application
  ↓
Domain

Infrastructure implements the technical services required by the Application layer.
```

### Layer Responsibilities

- **API** — Receives HTTP requests and returns HTTP responses.
- **Application** — Coordinates workflows, validation, DTOs, services, and repository contracts.
- **Domain** — Contains business entities and business rules.
- **Infrastructure** — Implements database access, password hashing, EF Core, and external integrations.

## Project Structure

```text
AIResumeCareerPlatform
├── backend
│   ├── src
│   │   ├── AIResume.API
│   │   ├── AIResume.Application
│   │   ├── AIResume.Domain
│   │   └── AIResume.Infrastructure
│   └── AIResumeCareerPlatform.slnx
├── frontend
├── docs
├── tests
├── .gitignore
└── README.md
```

## User Registration Flow

```text
React
  ↓
UsersController
  ↓
IUserRegistrationService
  ↓
UserRegistrationService
  ↓
RegisterUserRequestValidator
  ↓
IPasswordHasherService
  ↓
IUserRepository
  ↓
UserRepository
  ↓
AppDbContext
  ↓
Entity Framework Core
  ↓
SQL Server
```

## Registration Endpoint

```http
POST /api/users/register
```

Example request:

```json
{
  "firstName": "Ram",
  "lastName": "Reddy",
  "email": "ram@example.com",
  "password": "Password123"
}
```

Example successful response:

```json
{
  "id": "generated-guid",
  "firstName": "Ram",
  "lastName": "Reddy",
  "email": "ram@example.com",
  "createdAtUtc": "2026-08-06T18:39:00Z"
}
```

The password and password hash are never returned in the API response.

## Run the Backend

Move to the backend folder:

```powershell
cd C:\RamProjects\AIResumeCareerPlatform\backend
```

Restore dependencies:

```powershell
dotnet restore
```

Build the solution:

```powershell
dotnet build
```

Run the API:

```powershell
dotnet run --project src\AIResume.API\AIResume.API.csproj
```

The API runs on the localhost port shown in the terminal.

Example:

```text
http://localhost:5269
```

## Database Migrations

Create a migration:

```powershell
dotnet ef migrations add MigrationName `
  --project src\AIResume.Infrastructure\AIResume.Infrastructure.csproj `
  --startup-project src\AIResume.API\AIResume.API.csproj `
  --output-dir Persistence\Migrations
```

Apply migrations:

```powershell
dotnet ef database update `
  --project src\AIResume.Infrastructure\AIResume.Infrastructure.csproj `
  --startup-project src\AIResume.API\AIResume.API.csproj
```

## API Testing

A registration request can be tested using the `AIResume.API.http` file:

```http
POST http://localhost:5269/api/users/register
Content-Type: application/json

{
  "firstName": "Ram",
  "lastName": "Reddy",
  "email": "ram@example.com",
  "password": "Password123"
}
```

A successful request returns:

```text
HTTP/1.1 201 Created
```

## Security

- Plain-text passwords are never stored.
- Passwords are converted into secure password hashes.
- Sensitive fields are not returned through response DTOs.
- Generated folders and secret files are excluded through `.gitignore`.

## Author

**Ram Reddy**

GitHub: `ramreddyjagurampally`