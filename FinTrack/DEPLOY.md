# Deploying FinTrack to Azure

## 1. Create Azure resources
- Resource Group: `fintrack-rg`
- Azure SQL Database: server `fintrack-sql-server`, database `FinTrackDb`
- App Service `fintrack-api` — .NET 10 runtime
- App Service `fintrack-client` — .NET 10 runtime

## 2. Configure the API App Service
In **Configuration → Connection strings**, add:
- Name: `DefaultConnection`
- Value: `Server=fintrack-sql-server.database.windows.net;Database=FinTrackDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;`
- Type: `SQLAzure`

In **Configuration → Application settings**, add:
- `Jwt__Key` = `FinTrack_SuperSecret_JWT_Key_2025_BYU_Idaho_CSE325`
- `Jwt__Issuer` = `FinTrack.Server`
- `Jwt__Audience` = `FinTrack.Client`

## 3. Update the client API URL
In `FinTrack.Client/wwwroot/appsettings.json`:
```json
{ "ApiBaseUrl": "https://fintrack-api.azurewebsites.net/" }
```

## 4. Publish
Right-click each project in Visual Studio → **Publish** → Azure → App Service → select the matching service → Publish.

Or via CLI:
```bash
cd FinTrack.Server
dotnet publish -c Release -o ./publish
az webapp deploy --resource-group fintrack-rg --name fintrack-api --src-path ./publish

cd ../FinTrack.Client
dotnet publish -c Release -o ./publish
az webapp deploy --resource-group fintrack-rg --name fintrack-client --src-path ./publish
```

## 5. Update CORS
In `Program.cs`, make sure the deployed client URL is in the CORS policy, then republish the server.
