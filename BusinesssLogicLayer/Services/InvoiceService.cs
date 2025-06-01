using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.InvoiceDtos;


namespace BusinesssLogicLayer.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public InvoiceService(HttpClient httpClient,
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

        public async Task<string> GetInvoicesAsync(string? cardCode, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(cardCode))
                filters.Add($"CardCode eq '{cardCode}'");

            if (fromDate.HasValue)
                filters.Add($"DocDate ge {fromDate.Value:yyyy-MM-dd}");

            if (toDate.HasValue)
                filters.Add($"DocDate le {toDate.Value:yyyy-MM-dd}");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponce(response);

        }

        public async Task<string> PostInvoicesAsync(Invoice invoice)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices";

            var json = JsonSerializer.Serialize(invoice, new JsonSerializerOptions { PropertyNamingPolicy = null });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponce(response);
        }

        public async Task<string> PatchInvoicesAsync(int docEntry, UpdateInvoiceDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices({docEntry})";

            var payload = new Dictionary<string, object>();

            if (updateDto.DocDueDate.HasValue)
                payload["DocDueDate"] = updateDto.DocDueDate.Value.ToString("yyyy-MM-dd");

            if (updateDto.DocDate.HasValue)
                payload["DocDate"] = updateDto.DocDate.Value.ToString("yyyy-MM-dd");

            if (payload.Count == 0)
                throw new Exception("Nothing to update");

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            return await HandleResponce(response);

        }

        public async Task<string> CancelInvoiceAsync(int docEntry)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices({docEntry})/Cancel";

            var response = await _httpClient.PostAsync(url, null);

            return await HandleResponce(response);
        }

        public async Task<string> GetInvoiceByIdAsync(int cardCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices({cardCode})";

            var result = await _httpClient.GetAsync(url);

            return await HandleResponce(result);
        }

        public async Task<string> GetInvoiceExpandIncomingPayments(int cardCode)
        {
            SetCookiesHeader();

            var ulr = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$filter=DocEntry eq {cardCode}&$expand=IncomingPayments";

            var result = await _httpClient.GetAsync(ulr);

            return await HandleResponce(result);

        }
    }
}
