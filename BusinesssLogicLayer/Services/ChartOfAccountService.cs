using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.ChartOfAccountDtos;

namespace BusinesssLogicLayer.Services
{
    public class ChartOfAccountService : IChartOfAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public ChartOfAccountService(HttpClient httpClient, 
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

        public async Task<string> DeleteChartOfAccountsAsync(string accode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/ChartOfAccounts('{accode}')";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }

        public async Task<string> GetChartOfAccountsAsync(string? acctCode, string? acctName, int page = 1, int pageSize = 1)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(acctCode))
                filters.Add($"contains(Code, '{acctCode}')");

            if (!string.IsNullOrWhiteSpace(acctName))
                filters.Add($"contains(Name, '{acctName}')");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/ChartOfAccounts?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);

            return await HandleResponce(response);
        }

        public async Task<string> PatchChartOfAccountsAsync(string accode, UpdateChartOfAccountDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/ChartOfAccounts('{accode}')";

            var payload = new Dictionary<string, object>();

            // Faqat null yoki default qiymat bo'lmagan maydonlarni qo'shamiz
            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                payload["Name"] = updateDto.Name;

            if (!string.IsNullOrWhiteSpace(updateDto.AccountType))
                payload["AccountType"] = updateDto.AccountType;

            if (updateDto.AccountLevel > 0) // Agar 0 dan katta bo'lsa
                payload["AccountLevel"] = updateDto.AccountLevel;

            if (!string.IsNullOrWhiteSpace(updateDto.CashAccount))
                payload["CashAccount"] = updateDto.CashAccount;

            // Balance uchun null mumkin emas, lekin 0 bo'lsa o'zgarmasdan qoldirish mumkin
            // Agar 0 dan farqli qiymat kelgan bo'lsa, qo'shish
            if (updateDto.Balance != 0.0)
                payload["Balance"] = updateDto.Balance;

            if (!string.IsNullOrWhiteSpace(updateDto.FatherAccountKey))
                payload["FatherAccountKey"] = updateDto.FatherAccountKey;

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }

        public async Task<string> PostChartOfAccountsAsync(ChartOfAccounts body)
        {
            SetCookiesHeader();

            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/ChartOfAccounts";

            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // bu eng MUHIM QISM
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponce(response);

        }

        public async Task<string> GetChartsOfAccountsByIdAsync(string code)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/ChartOfAccounts('{code}')";

            var response = await    _httpClient.GetAsync(url);
            return await HandleResponce(response);
        }
    }
}
