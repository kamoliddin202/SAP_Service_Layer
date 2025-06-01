using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.SupplierDtos;

namespace BusinesssLogicLayer.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public SupplierService(HttpClient httpClient,
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
                throw new Exception($"SAP error : {result}");
            return result;
        }

        public async Task<string> DeleteSupplierAsync(string cardCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{cardCode}')";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse(response);
        }

        public async Task<string> GetSuppliersAsync(string? cardCode, string? cardName, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();
            if (!string.IsNullOrWhiteSpace(cardCode))
                filters.Add($"contains(CardCode, '{cardCode}')");
            if (!string.IsNullOrWhiteSpace(cardName))
                filters.Add($"contains(CardName, '{cardName}')");

            string filterQuery = filters.Count > 0 ? "$filter=" + string.Join(" and ", filters) + "&" : "";
            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> PatchSupplierAsync(string cardCode, UpdateSupplierDto dto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{cardCode}')";

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(dto.CardName))
                payload["CardName"] = dto.CardName;

            if (!string.IsNullOrEmpty(dto.CardType))
                payload["CardType"] = dto.CardType;

            if (!string.IsNullOrEmpty(dto.Currency))
                payload["Currency"] = dto.Currency;

            if (!string.IsNullOrEmpty(dto.Phone1))
                payload["Phone1"] = dto.Phone1;

            if (!string.IsNullOrEmpty(dto.EmailAddress))
                payload["EmailAddress"] = dto.EmailAddress;

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

        public async Task<string> PostSupplierAsync(Supplier supplier)
        {
            SetCookiesHeader();

            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners";

            var json = JsonSerializer.Serialize(supplier, new JsonSerializerOptions { PropertyNamingPolicy = null });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponse(response);
        }

        public async Task<string> GetSuplierByIdAsync(string supplierId)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners('{supplierId}')";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }
    }
}
