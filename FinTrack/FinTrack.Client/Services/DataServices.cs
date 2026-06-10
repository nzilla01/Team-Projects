using FinTrack.Shared.DTOs;
using System.Net.Http.Json;

namespace FinTrack.Client.Services
{
    public class TransactionService
    {
        private readonly HttpClient _http;
        public TransactionService(HttpClient http) => _http = http;

        public async Task<List<TransactionDto>> GetAll(int? categoryId = null, string? type = null)
        {
            var url = "api/transactions";
            var query = new List<string>();
            if (categoryId.HasValue) query.Add($"categoryId={categoryId}");
            if (!string.IsNullOrEmpty(type)) query.Add($"type={type}");
            if (query.Any()) url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<List<TransactionDto>>(url) ?? new();
        }

        public async Task<TransactionDto?> GetById(int id)
            => await _http.GetFromJsonAsync<TransactionDto>($"api/transactions/{id}");

        public async Task<bool> Create(CreateTransactionDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/transactions", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, CreateTransactionDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/transactions/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _http.DeleteAsync($"api/transactions/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    public class DashboardService
    {
        private readonly HttpClient _http;
        public DashboardService(HttpClient http) => _http = http;

        public async Task<DashboardDto?> Get()
            => await _http.GetFromJsonAsync<DashboardDto>("api/dashboard");
    }

    public class SavingsGoalService
    {
        private readonly HttpClient _http;
        public SavingsGoalService(HttpClient http) => _http = http;

        public async Task<List<SavingsGoalDto>> GetAll()
            => await _http.GetFromJsonAsync<List<SavingsGoalDto>>("api/savingsgoals") ?? new();

        public async Task<bool> Create(CreateSavingsGoalDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/savingsgoals", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Deposit(int id, decimal amount)
        {
            var response = await _http.PutAsJsonAsync($"api/savingsgoals/{id}/deposit", amount);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _http.DeleteAsync($"api/savingsgoals/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    public class CategoryService
    {
        private readonly HttpClient _http;
        public CategoryService(HttpClient http) => _http = http;

        public async Task<List<CategoryDto>> GetAll()
            => await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? new();
    }
}
