using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IoTAuth.Supabase
{
    public class SupabaseOptions
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }

    public interface ISupabaseService
    {
        Task<string> SignUpAsync(string email, string password);
        Task<bool> AddUserAsync(string accessToken, string username, string fullName);
        Task<string?> SignInAsync(string email, string password);
    }


    public class SupabaseService : ISupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseOptions _options;

        public SupabaseService(HttpClient httpClient, IOptions<SupabaseOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri(_options.Url);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("apikey", _options.ApiKey);
        }

        public async Task<string?> SignInAsync(string email, string password)
        {
            var payload = new
            {
                email,
                password
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/auth/v1/token?grant_type=password", content);

            if (!response.IsSuccessStatusCode)
                return null;

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("access_token").GetString();
        }


        public async Task<string> SignUpAsync(string email, string password)
        {
            var payload = new
            {
                email,
                password
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/auth/v1/signup", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString();

            return accessToken;
        }

        public async Task<bool> AddUserAsync(string accessToken, string username, string fullName)
        {
            var payload = new
            {
                username,
                full_name = fullName
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/Users");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("apikey", _options.ApiKey);
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
