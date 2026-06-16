# FinTrack — Personal Finance Tracker
### CSE 325: .NET Software Development | BYU-Idaho | 2025

> A full-stack personal finance web application built with .NET 10 Blazor WebAssembly and ASP.NET Core.

---

## Team

| Name | Role |
|------|------|
| Nsikak Okon | Team Lead / Backend & Authentication |
| Victor | Frontend UI / Blazor Components |
| Clement | Database / CRUD APIs / Deployment |

---

## Project Links

| Resource | URL |
|----------|-----|
| GitHub Repository | https://github.com/nzilla01/Team-Projects |
| Trello Board | https://trello.com/b/QCX9mZ7L/financial-journal |
| Live App | https://fintrack-client.azurewebsites.net |
| Demo Video | [YouTube link — added after recording] |

---

## Features

- **User Authentication** — register, login, logout with JWT tokens
- **Transactions** — add, edit, delete income and expense records
- **Category Tagging** — 10 pre-loaded categories (Food, Rent, Salary, etc.)
- **Dashboard** — live balance, recent transactions, spending breakdown
- **Savings Goals** — create targets, deposit funds, track progress
- **Responsive Design** — works on desktop, tablet, and mobile

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | .NET 10 Blazor WebAssembly |
| Backend | ASP.NET Core 10 Web API |
| Database | Entity Framework Core + SQL Server |
| Authentication | ASP.NET Core Identity + JWT |
| Deployment | Microsoft Azure App Service |
| Version Control | GitHub |
| Project Management | Trello |

---

## Local Setup

### Requirements
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server or SQL Server Express (LocalDB)
- VS Code or Visual Studio 2022

### Run Locally

```bash
# 1. Clone
git clone https://github.com/nzilla01/Team-Projects.git
cd Team-Projects/FinTrack

# 2. Restore packages
dotnet restore

# 3. Create and migrate database
cd FinTrack.Server
dotnet ef database update

# 4. Run server (Terminal 1)
dotnet run
# Runs at https://localhost:7000

# 5. Run client (Terminal 2)
cd ../FinTrack.Client
dotnet run
# Runs at https://localhost:7001
```

Open **https://localhost:7001** in your browser.

---

## Project Structure

```
FinTrack/
├── FinTrack.Server/          # ASP.NET Core API
│   ├── Controllers/          # Auth, Transactions, Categories, Goals, Dashboard
│   ├── Data/                 # EF Core DbContext + Migrations
│   ├── Migrations/           # Database migration files
│   ├── appsettings.json      # Local configuration
│   └── Program.cs            # App startup & middleware
│
├── FinTrack.Client/          # Blazor WebAssembly frontend
│   ├── Pages/                # Dashboard, Transactions, Goals, Login, Register
│   ├── Services/             # AuthService, TransactionService, etc.
│   ├── Shared/               # MainLayout, EmptyLayout, RedirectToLogin
│   └── wwwroot/              # index.html, app.css
│
├── FinTrack.Shared/          # Shared models and DTOs
│   ├── Models/               # Transaction, Category, SavingsGoal, AppUser
│   └── DTOs/                 # Data transfer objects for API communication
│
├── USER_GUIDE.md             # End-user documentation
├── DEPLOY.md                 # Azure deployment instructions
└── README.md                 # This file
```

---

## Deploy to Azure

See **DEPLOY.md** for full step-by-step Azure deployment instructions.

---

*© 2025 Nsikak Okon, Victor, Clement — CSE 325 BYU-Idaho*
