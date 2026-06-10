# FinTrack — Personal Finance Tracker

> A .NET Blazor web application for tracking income, expenses, and savings goals.
> Built as a group project for CSE 325: .NET Software Development at BYU-Idaho.

---

## Team Members

| Name | Role |
|------|------|
| Nsikak Okon | Team Lead / Backend & Auth |
| Victor | Frontend UI / Blazor Components |
| Clement | Database / CRUD & Deployment |

---

## Project Overview

FinTrack is a full-stack personal finance tracking web application that helps college students and young adults manage their money. Users can log income and expenses, organize transactions by category, set savings goals, and visualize their spending with charts — all behind a secure login.

---

## Features

- **User authentication** — register, login, and manage your account securely
- **Transaction management** — add, edit, and delete income and expense records
- **Category tagging** — organize transactions by category (food, rent, transport, etc.)
- **Dashboard overview** — see your balance, recent transactions, and spending at a glance
- **Savings goals** — set targets and track progress toward financial goals
- **Spending charts** — visualize monthly spending by category
- **Responsive design** — works across desktop, tablet, and mobile devices

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | .NET Blazor WebAssembly |
| Backend | ASP.NET Core Web API |
| Database | Entity Framework Core + SQL Server |
| Authentication | ASP.NET Core Identity |
| Hosting | Azure App Service |
| Version Control | GitHub |
| Project Management | Trello |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server Express
- Git

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/nzilla01/Team-Projects 
   cd fintrack
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database connection**

   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FinTrackDb;Trusted_Connection=True;"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Open in browser**

   Navigate to `https://localhost:5001`


## Project Links

- **Trello Board:** `[link to be added]`
- **GitHub Repository:** `[link to be added]`
- **Live Deployed App:** `[link to be added]`
- **Demo Video:** `[link to be added]`

---

## Usage Guide

1. **Register** — create an account with your email and password
2. **Log in** — sign in to access your personal dashboard
3. **Add a transaction** — click "Add Transaction", fill in amount, category, date, and description
4. **View your dashboard** — see your balance summary and recent activity
5. **Set a savings goal** — navigate to Goals, enter a target amount and deadline
6. **View spending charts** — go to Reports to see your spending broken down by category

---

## Development Notes

- All API endpoints are protected with JWT authentication
- CRUD operations are available for transactions, categories, and savings goals
- The app is tested with Lighthouse for performance and accessibility (WCAG 2.1 AA)
- Responsive design is built with CSS Flexbox and Grid

---

## License

This project was created for academic purposes as part of CSE 325 at BYU-Idaho.

© 2025 Nsikak Okon, Victor, Clement — All rights reserved.
