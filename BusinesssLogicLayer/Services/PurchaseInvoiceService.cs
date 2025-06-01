using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.PurchaseOrderModels;

namespace BusinesssLogicLayer.Services
{
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public PurchaseInvoiceService(HttpClient httpClient,
                                SapSession sapSession)
        {
            _httpClient = httpClient;
            _sapSession = sapSession;
        }

        private void SetCookiesHeader()
        {
            _httpClient.DefaultRequestHeaders.Remove("Cookie");

            var cookies = _sapSession.GetCookies();

            // cache'dan olib kelinadi

            if (!string.IsNullOrEmpty(cookies))
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
            }
        }

        private async Task<string> HandleResponce(HttpResponseMessage responce)
        {
            var result = await responce.Content.ReadAsStringAsync();
            if (!responce.IsSuccessStatusCode)
            {
                throw new Exception($"SAP error : {result}");
            }

            return result;
        }

        public async Task<string> DeletePurchaseInvoiceAsync(int docEntiry)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices({docEntiry})";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);
            return await HandleResponce(response);
        }

        public async Task<string> GetPurchaseInvoiceAsync(string? cardCode, string? docNum, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(cardCode))
                filters.Add($"contains(CardCode, '{cardCode}')");

            if (!string.IsNullOrWhiteSpace(docNum))
                filters.Add($"DocNum eq {docNum}");

            if (fromDate.HasValue)
                filters.Add($"DocDate ge {fromDate.Value:yyyy-MM-dd}");

            if (toDate.HasValue)
                filters.Add($"DocDate le {toDate.Value:yyyy-MM-dd}");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);

            return await HandleResponce(response);
        }

        public async Task<string> PatchPurchaseInvoiceAsync(int docEntry, UpdatePurchaseInvoiceDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices({docEntry})";

            var payload = new Dictionary<string, object>
            {
                ["DocDate"] = updateDto.DocDate.ToString("yyyy-MM-dd")
            };

            // Agar DocDueDate nullable bo‘lsa:
            if (updateDto.DocDueDate.HasValue)
            {
                payload["DocDueDate"] = updateDto.DocDueDate.Value.ToString("yyyy-MM-dd");
            }

            // Agar DocDueDate majburiy bo‘lsa (nullable emas):
            // payload["DocDueDate"] = updateDto.DocDueDate.ToString("yyyy-MM-dd");

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }

        public async Task<string> PostPurchaseInvoiceAsync(PurchaseInvoice body)
        {
            SetCookiesHeader();

            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices";

            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = null });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponce(response);

        }

        public async Task<string> GetPurchaseInvoiceByIdAsync(int cardCode)
        {
            SetCookiesHeader(); 

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices({cardCode})";

            var responce = await _httpClient.GetAsync(url);

            return await HandleResponce(responce);

        }
    }
}
