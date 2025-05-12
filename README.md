# JobNest

**JobNest** is an ASP.NET Core Web API for managing job listings and applications. It enables employers to create jobs and track applications, while users can browse jobs, apply with resumes, and submit cover letters.

## ✅ Admin/Employer Can:

- Post and manage jobs  
- View applicants for each job  
- Receive applications with resumes and cover letters  

## 🙋 User Can:

- View available job listings  
- Apply for a job using their profile  
- Upload a cover letter via URL (optional)  
- Submit their resume automatically from their profile  

## 📦 Clean Architecture Structure

- Controllers  
- Services  
- DTOs (Request/Response Models)  
- Interfaces  
- Entities  
- Token & Auth Layer  
- Uses dependency injection to separate concerns

## 💾 Entity Framework Core

- Code-First approach  
- SQL Server or LocalDb  
- Migrations supported via CLI  

## 📚 Swagger UI

- Interactive documentation via Swagger  
- Testable endpoints for developers and testers

## 🛠️ Tech Stack

- ASP.NET Core Web API (.NET 7)  
- Entity Framework Core  
- SQL Server  
- JWT Authentication  
- Swagger / Swashbuckle

## 📌 Notes

- JWT token required for secure endpoints  
- Resume pulled from authenticated user's profile  
- Cover letter submitted via URL  

## 🤝 Contributions

I welcome pull requests, issues, and feedback!  
Help improve JobNest for job seekers and recruiters worldwide.

## 🔒 Authentication Flow

- JWT-based authentication  
- Claims include `UserId`, `Email`, and `UserName`  
- Token is extracted from Authorization headers (`Bearer {token}`)
