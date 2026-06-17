using Blazored.LocalStorage;
using FinTrack.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<FinTrack.Client.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Base API URL — reads from wwwroot/appsettings.json
var apiBase = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

// Auth services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<AuthService>();

// Feature services
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<SavingsGoalService>();
builder.Services.AddScoped<CategoryService>();

await builder.Build().RunAsync();
