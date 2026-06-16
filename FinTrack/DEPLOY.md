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

## 2. Deploy to Azure

### Step 1 — Create Azure Resources (Azure Portal)

1. Go to [portal.azure.com](https://portal.azure.com)
2. Create a **Resource Group**: `fintrack-rg`
3. Create an **Azure SQL Database**:
   - Server name: `fintrack-sql-server`
   - Database name: `FinTrackDb`
   - Authentication: SQL authentication
   - Note your **server name**, **username**, and **password**
4. Create an **Azure App Service** (for the API):
   - Name: `fintrack-api`
   - Runtime: `.NET 8`
   - OS: Windows or Linux
   - Plan: Free F1 (for demo) or B1 (for production)
5. Create a second **Azure App Service** (for the Blazor client):
   - Name: `fintrack-client`
   - Runtime: `.NET 8`

### Step 2 — Configure Connection String

In the `fintrack-api` App Service:
1. Go to **Configuration → Application Settings**
2. Add a new connection string:
   - Name: `DefaultConnection`
   - Value: `Server=fintrack-sql-server.database.windows.net;Database=FinTrackDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;`
   - Type: `SQLAzure`
3. Add application settings:
   - `Jwt__Key` = `FinTrack_SuperSecret_JWT_Key_2025_BYU_Idaho_CSE325`
   - `Jwt__Issuer` = `FinTrack.Server`
   - `Jwt__Audience` = `FinTrack.Client`

### Step 3 — Update Client API URL

In `FinTrack.Client/wwwroot/appsettings.json`, change:
```json
{
  "ApiBaseUrl": "https://fintrack-api.azurewebsites.net/"
}
```

### Step 4 — Publish via Visual Studio

**Publish the Server:**
1. Right-click `FinTrack.Server` → Publish
2. Target: Azure → Azure App Service (Windows)
3. Select `fintrack-api`
4. Click Publish

**Publish the Client:**
1. Right-click `FinTrack.Client` → Publish
2. Target: Azure → Azure App Service (Windows)
3. Select `fintrack-client`
4. Click Publish

### Step 5 — Update CORS (Server)

In `FinTrack.Server/Program.cs`, update the CORS policy to include your deployed client URL:
```csharp
.WithOrigins("https://fintrack-client.azurewebsites.net")
```
Then republish the server.

---

## 3. Publish via Azure CLI (Alternative)

```bash
# Login
az login

# Publish Server
cd FinTrack.Server
dotnet publish -c Release -o ./publish
az webapp deploy --resource-group fintrack-rg --name fintrack-api --src-path ./publish

# Publish Client
cd ../FinTrack.Client
dotnet publish -c Release -o ./publish
az webapp deploy --resource-group fintrack-rg --name fintrack-client --src-path ./publish
```

---

## 4. Project Links

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
