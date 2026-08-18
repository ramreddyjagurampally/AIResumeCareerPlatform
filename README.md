# AI Resume Career Platform

A full-stack career platform that allows users to securely upload resumes, extract PDF content, perform ATS-style resume analysis, and compare resumes against job descriptions.

## Live Demo

Frontend:

https://nice-dune-038ee5e10.7.azurestaticapps.net

## Overview

AI Resume Career Platform is a full-stack web application built with React, TypeScript, ASP.NET Core, Entity Framework Core, SQL Server, and Microsoft Azure.

The application demonstrates secure authentication, REST API development, resume processing, ATS-style analysis, job matching, database integration, and cloud deployment.

> Note: The current resume analysis and job matching logic is rule-based and does not use an external AI or LLM model.

## Features

- User registration
- Secure login
- JWT authentication
- Protected user profile
- PDF resume upload
- User-specific resume ownership
- PDF text extraction
- ATS-style resume score
- Technical skill detection
- Resume strengths detection
- Missing section detection
- Resume improvement suggestions
- Job description matching
- Match percentage
- Matched skill detection
- Missing skill detection
- Job-specific recommendations
- Responsive frontend interface
- Azure cloud deployment

## Tech Stack

### Frontend

- React
- TypeScript
- Vite
- React Router
- CSS
- Fetch API

### Backend

- C#
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- REST APIs
- Dependency Injection
- JWT Authentication

### Database

- SQL Server
- Azure SQL Database
- Entity Framework Core Migrations

### Resume Processing

- PdfPig
- Custom rule-based ATS analysis
- Skill detection
- Resume section detection
- Job-description keyword matching

### Cloud

- Azure App Service
- Azure SQL Database
- Azure Static Web Apps

## Application Architecture

The backend follows a layered architecture.

```text
React Frontend
      |
      v
ASP.NET Core API
      |
      v
Application Layer
      |
      v
Domain Layer
      |
      v
Infrastructure Layer
      |
      v
Azure SQL Database