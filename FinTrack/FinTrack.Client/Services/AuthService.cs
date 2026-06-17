using Blazored.LocalStorage;
using FinTrack.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace FinTrack.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public JwtAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (string.IsNullOrWhiteSpace(token)) return Unauthenticated();

            var claims = ParseClaimsFromJwt(token);
            if (claims == null) return Unauthenticated();

            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out var exp))
            {
                var expiry = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                if (expiry < DateTime.UtcNow)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    return Unauthenticated();
                }
            }

            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token) ?? new List<Claim>();
            var identity = new ClaimsIdentity(claims, "jwt");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void NotifyUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        private static AuthenticationState Unauthenticated()
            => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3) return null;

                var payload = parts[1].Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                var kvp = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
                if (kvp == null) return null;

                var claims = new List<Claim>();
                foreach (var pair in kvp)
                {
                    var value = pair.Value.ValueKind == JsonValueKind.String
                        ? pair.Value.GetString() ?? "" : pair.Value.ToString();
                    claims.Add(new Claim(pair.Key, value));
                }

                var sub = claims.FirstOrDefault(c => c.Type == "sub");
                if (sub != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, sub.Value));

                var email = claims.FirstOrDefault(c => c.Type == "email");
                if (email != null) claims.Add(new Claim(ClaimTypes.Email, email.Value));

                return claims;
            }
            catch { return null; }
        }
    }

    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly JwtAuthStateProvider _authProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authProvider = (JwtAuthStateProvider)authProvider;
        }

        public async Task<(bool Success, string Message)> Register(RegisterDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, error?.Message ?? "Registration failed.");
            }
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            await StoreToken(result!);
            return (true, "Registered successfully.");
        }

        public async Task<(bool Success, string Message)> Login(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, error?.Message ?? "Invalid email or password.");
            }
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            await StoreToken(result!);
            return (true, "Logged in successfully.");
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userInfo");
            _http.DefaultRequestHeaders.Authorization = null;
            _authProvider.NotifyUserLoggedOut();
        }

        private async Task StoreToken(AuthResponseDto auth)
        {
            await _localStorage.SetItemAsStringAsync("authToken", auth.Token);
            await _localStorage.SetItemAsync("userInfo", auth);
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth.Token);
            _authProvider.NotifyUserLoggedIn(auth.Token);
        }

        public async Task<AuthResponseDto?> GetCurrentUser()
            => await _localStorage.GetItemAsync<AuthResponseDto>("userInfo");
    }

    public class ErrorResponse { public string Message { get; set; } = ""; }
}
