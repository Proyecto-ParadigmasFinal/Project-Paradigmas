using Proyecto_Paradigmas.Dtos.Payments;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Proyecto_Paradigmas.Services.Interfaces;



namespace Proyecto_Paradigmas.Services
{
    public class PaypalService : IPaypalServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaypalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PaymentResponseDto> CreateOrderAsync(PaymentCreateDto request)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orderPayload = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = request.PrecioTotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(orderPayload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["PayPalSettings:UrlBase"]}/v2/checkout/orders", content);

            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<JsonElement>();

            var orderId = responseData.GetProperty("id").GetString()!;
            var links = responseData.GetProperty("links").EnumerateArray();
            var approvalLink = links.FirstOrDefault(l => l.GetProperty("rel").GetString() == "approve").GetProperty("href").GetString()!;

            return new PaymentResponseDto { OrderId = orderId, ApprovalLink = approvalLink };
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["PayPalSettings:UrlBase"]}/v2/checkout/orders/{orderId}/capture", content);

            return response.IsSuccessStatusCode;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var clientId = _configuration["PayPalSettings:ClientId"];
            var secret = _configuration["PayPalSettings:Secret"];
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync($"{_configuration["PayPalSettings:UrlBase"]}/v1/oauth2/token", content);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<JsonElement>();
            return data.GetProperty("access_token").GetString()!;
        }
    }
}
