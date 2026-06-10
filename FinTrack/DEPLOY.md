# FinTrack — Build & Deploy to Azure
## CSE 325 Group Project | Nsikak Okon, Victor, Clement

---

## 1. Local Setup (Run on Your Machine First)

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) OR [VS Code](https://code.visualstudio.com/)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB included with VS 2022)
- [Git](https://git-scm.com/)

### Steps

```bash
# 1. Clone the repo
git clone https://github.com/nzilla01/Team-Projects.git
cd Team-Projects

# 2. Restore all packages
dotnet restore

# 3. Apply database migrations (creates the DB automatically)
cd FinTrack.Server
dotnet ef database update

# 4. Run the server
dotnet run
# Server runs at https://localhost:7000

# 5. In a second terminal, run the client
cd ../FinTrack.Client
dotnet run
# Client runs at https://localhost:7001
```

Open https://localhost:7001 in your browser — you should see the FinTrack login page.


---

## 2. Project Links

| Resource | URL |
|----------|-----|
| GitHub Repo | https://github.com/nzilla01/Team-Projects |
| Trello Board | https://trello.com/b/QCX9mZ7L/financial-journal |
| Live App | https://fintrack-client.azurewebsites.net (add after deploy) |

---

## Team

| Name | Role |
|------|------|
| Nsikak Okon | Team Lead / Backend & Auth |
| Victor | Frontend UI / Blazor |
| Clement | Database & Deployment |
