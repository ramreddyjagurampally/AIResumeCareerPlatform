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

### Architecture

The backend follows Clean Architecture:

```text
API
  ↓
Application
  ↓
Domain

Infrastructure implements technical services required by Application.
