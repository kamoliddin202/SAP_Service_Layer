using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;

namespace BusinesssLogicLayer.Services
{
    public class SapQueryService : ISapQueryInterface
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public SapQueryService(HttpClient httpClient, SapSession sapSession)
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

        public async Task<string> GetInvoicesAsync(string? cardCode, string? docDateFrom, string? docDateTo, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();
            if (!string.IsNullOrWhiteSpace(cardCode))
                filters.Add($"contains(CardCode, '{cardCode}')");

            if (!string.IsNullOrWhiteSpace(docDateFrom))
                filters.Add($"DocDate ge datetime'{docDateFrom}T00:00:00'");

            if (!string.IsNullOrWhiteSpace(docDateTo))
                filters.Add($"DocDate le datetime'{docDateTo}T23:59:59'");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> GetInvoiceDetailsAsync(int docEntry)
        {
            SetCookiesHeader();

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices({docEntry})";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> GetMonthlyPaymentsAsync(string cardCode, int year, int month)
        {
            SetCookiesHeader();

            var fromDate = new DateTime(year, month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);

            string dateFrom = fromDate.ToString("yyyy-MM-dd");
            string dateTo = toDate.ToString("yyyy-MM-dd");

            // To‘g‘ri endpoint: IncomingPayments
            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments?$filter=CardCode eq '{cardCode}' and DocDate ge {dateFrom} and DocDate le {dateTo}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }


        public async Task<string> GetCustomersAsync(string? cardName)
        {
            SetCookiesHeader();

            string filter = string.IsNullOrWhiteSpace(cardName) ? "" : $"$filter=contains(CardName,'{cardName}')";
            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners?{filter}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }


        public async Task<string> GetPaymentsAsync(int docEntry)
        {
            SetCookiesHeader();

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/IncomingPayments?$filter=DocEntry eq {docEntry}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> GetPurchaseOrdersAsync(string? docNum, string? vendorName)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(docNum))
                filters.Add($"contains(DocNum, '{docNum}')");

            if (!string.IsNullOrWhiteSpace(vendorName))
                filters.Add($"contains(CardName, '{vendorName}')");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}" : "";

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseOrders?{filterQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> GetPurchaseOrderDetailsAsync(string docEntry)
        {
            SetCookiesHeader();

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseOrders({docEntry})";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> GetItemsAsync(string? itemCode, string? itemName)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(itemCode))
                filters.Add($"contains(ItemCode, '{itemCode}')");

            if (!string.IsNullOrWhiteSpace(itemName))
                filters.Add($"contains(ItemName, '{itemName}')");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}" : "";

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items?{filterQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }
    }
}



// 3 chisi ishlamayapti monthlypayed
// 5 chisi ishlamayapti 
