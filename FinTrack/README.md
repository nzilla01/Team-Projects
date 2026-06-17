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
| Live App | (added after Azure deployment) |
| Demo Video | (added after recording) |

---

## Features

- User authentication (register, login, logout) with JWT
- Transactions — add, edit, delete income and expense records
- Category tagging — 10 pre-loaded categories
- Dashboard — live balance, recent transactions, spending breakdown
- Savings Goals — create targets, deposit funds, track progress
- Responsive design for desktop, tablet, and mobile

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | .NET 10 Blazor WebAssembly |
| Backend | ASP.NET Core 10 Web API |
| Database | Entity Framework Core + SQL Server |
| Authentication | ASP.NET Core Identity + JWT |
| Deployment | Microsoft Azure App Service |

---

## Local Setup

```bash
git clone https://github.com/nzilla01/Team-Projects.git
cd Team-Projects/FinTrack
dotnet restore

cd FinTrack.Server
dotnet ef database update
dotnet run
# https://localhost:7000

# in a second terminal
cd ../FinTrack.Client
dotnet run
# https://localhost:7001
```

See `DEPLOY.md` for Azure deployment steps and `USER_GUIDE.md` for end-user documentation.

---

*© 2025 Nsikak Okon, Victor, Clement — CSE 325 BYU-Idaho*
