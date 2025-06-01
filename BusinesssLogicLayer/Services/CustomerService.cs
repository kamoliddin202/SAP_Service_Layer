using System.Text.Json;
using System.Text;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.SupplierDtos;
using DTOs.CutomerDtos;

namespace BusinesssLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public CustomerService(HttpClient httpClient, 
                                SapSession sapSession)
        {
            _httpClient = httpClient;
            _sapSession = sapSession;
        }

        private void SetCookiesHeader()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");

            var cookies = _sapSession.GetCookies();
            if (!string.IsNullOrEmpty(cookies))
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
            }
        }

        private async Task<string> HandleResponse(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"SAP error : {result}");
            }

            return result;
        }

        public async Task<string> GetCustomersAsync(string? cardCode, string? cardName, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>
            {
                "CardType eq 'C'" // faqat Customers
            };

            if (!string.IsNullOrWhiteSpace(cardCode))
                filters.Add($"contains(CardCode, '{cardCode}')");

            if (!string.IsNullOrWhiteSpace(cardName))
                filters.Add($"contains(CardName, '{cardName}')");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners?{filterQuery}{pagingQuery}";
            var response = await _httpClient.GetAsync(url);

            return await HandleResponse(response);
        }

        public async Task<string> PostCustomerAsync(Customer model)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners";
            var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponse(response);
        }

        public async Task<string> PatchCustomerAsync(string cardCode, UpdateCustomerDto dto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{cardCode}')";

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(dto.CardName))
                payload["CardName"] = dto.CardName;
            if (!string.IsNullOrWhiteSpace(dto.Currency))
                payload["Currency"] = dto.Currency;
            if (!string.IsNullOrWhiteSpace(dto.Phone1))
                payload["Phone1"] = dto.Phone1;
            if (!string.IsNullOrWhiteSpace(dto.EmailAddress))
                payload["EmailAddress"] = dto.EmailAddress;
            if (!string.IsNullOrEmpty(dto.CardType))
                payload["CardType"] = dto.CardType;

            if (payload.Count == 0)
                throw new Exception("Nothing to update");

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse(response);
        }

        public async Task<string> DeleteCustomerAsync(string cardCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{cardCode}')";
            var response = await _httpClient.DeleteAsync(url);

            return await HandleResponse(response);
        }

        public async Task<string> GetCustomerByIdAsync(string customerId)
        {
            SetCookiesHeader();

            var ulr = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{customerId}')";

            var response = await _httpClient.GetAsync(ulr);

            return await HandleResponse(response);
        }

        public async Task<string> GetCustemerWithInvoices(string cardCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners?$filter=CardCode eq '{cardCode}'&$expand=Invoices";

            var response =await _httpClient.GetAsync(url);

            return await HandleResponse(response);
        }
    }
}
