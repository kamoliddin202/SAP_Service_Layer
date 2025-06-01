using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.IncomingPaymentsDtos;
using Microsoft.Extensions.Logging.Abstractions;

namespace BusinesssLogicLayer.Services
{
    public class IncomingPaymentService : IIncomingPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public IncomingPaymentService(HttpClient httpClient, 
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

        public async Task<string> GetIncomingPaymentAsync(string? cardCode, string? docNum, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 100)
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

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponce(response);
        }

        public async Task<string> PostIncomingPaymentAsync(IncomingPayment body)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments";

            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // bu eng MUHIM QISM
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponce(response);
        }

        public async Task<string> PatchIncomingPaymentAsync(int docEntry, UpdateIncomingPaymentDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments({docEntry})";

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(updateDto.DocDate))
                payload["DocDate"] = updateDto.DocDate;

            // CashSum qiymati 0 bo'lishi mumkin, shuning uchun 0 dan katta bo'lsa qo'shamiz
            if (updateDto.CashSum > 0)
                payload["CashSum"] = updateDto.CashSum;

            if (!string.IsNullOrWhiteSpace(updateDto.DocType))
                payload["DocType"] = updateDto.DocType;

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }


        public async Task<string> DeleteIncomingPaymentAsync(int docEntry)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments({docEntry})";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }

        public async Task<string> GetIncomingPaymentsByIdAsync(string cardCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments({cardCode})";

            var response = await _httpClient.GetAsync(url);

            return await HandleResponce(response);

        }
    }
}
