using Blazored.LocalStorage;
using FinTrack.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace FinTrack.Client.Services
{
    /// <summary>
    /// Provides authentication state to Blazor components by reading
    /// the stored JWT token from local storage and parsing its claims.
    /// Uses a lightweight manual JWT parser — no external dependency needed.
    /// </summary>
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

            if (string.IsNullOrWhiteSpace(token))
                return Unauthenticated();

            var claims = ParseClaimsFromJwt(token);
            if (claims == null)
                return Unauthenticated();

            // Check expiry claim
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
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        private static AuthenticationState Unauthenticated()
            => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        /// <summary>
        /// Manually parses claims from a JWT payload without any external library.
        /// JWT structure: header.payload.signature (all base64url encoded)
        /// </summary>
        private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3) return null;

                // Base64url decode the payload (middle part)
                var payload = parts[1];
                // Pad base64 string to multiple of 4
                payload = payload.Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var json = System.Text.Encoding.UTF8.GetString(bytes);

                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
                if (keyValuePairs == null) return null;

                var claims = new List<Claim>();
                foreach (var kvp in keyValuePairs)
                {
                    var value = kvp.Value.ValueKind == JsonValueKind.String
                        ? kvp.Value.GetString() ?? ""
                        : kvp.Value.ToString();
                    claims.Add(new Claim(kvp.Key, value));
                }

                // Map standard JWT claims to ClaimTypes
                var sub = claims.FirstOrDefault(c => c.Type == "sub");
                if (sub != null)
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, sub.Value));

                var email = claims.FirstOrDefault(c => c.Type == "email");
                if (email != null)
                    claims.Add(new Claim(ClaimTypes.Email, email.Value));

                return claims;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Handles registration and login API calls.
    /// Stores the returned JWT in local storage and updates auth state.
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly JwtAuthStateProvider _authProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage,
            AuthenticationStateProvider authProvider)
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
