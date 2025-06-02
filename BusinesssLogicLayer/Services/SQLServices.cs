
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

public class SQLServices : ISQLInterfaces
{
    private readonly HttpClient _httpClient;
    private readonly SapSession _sapSession;

    public SQLServices(HttpClient httpClient, SapSession sapSession)
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

    private async Task<string> ExecuteAllSqlAsync(string sqlText)
    {
        SetCookiesHeader();

        var timestamp = DateTime.UtcNow.Ticks;
        var queryCode = $"Query_{timestamp}";

        var sqlQuery = new
        {
            SqlCode = queryCode,
            SqlName = queryCode,
            SqlText = sqlText
        };

        var json = System.Text.Json.JsonSerializer.Serialize(sqlQuery, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var postUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/SQLQueries";
        var postResponse = await _httpClient.PostAsync(postUrl, content);
        if (!postResponse.IsSuccessStatusCode)
            return await postResponse.Content.ReadAsStringAsync();

        var getUrl = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/SQLQueries('{queryCode}')/List";
        var getResponse = await _httpClient.GetAsync(getUrl);
        return await getResponse.Content.ReadAsStringAsync();
    }

    public async Task<string> UmumiyNarx500danKattaSotuvlarAsync()
    {
        string sql = @"
        SELECT ""DocEntry"", ""DocNum"", ""CardCode"", ""CardName"", ""DocDate"", ""DocTotal""
        FROM ""OINV""
        WHERE ""DocTotal"" > 500
        ORDER BY ""DocDate"" DESC";

        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> UmumiyNarx500danKattaMijozIsimlariBilanAsync()
    {
        string sql = @"
        SELECT O.""DocNum"", O.""DocDate"", O.""DocTotal"", C.""CardName""
        FROM ""OINV"" O
        INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode""
        WHERE O.""DocTotal"" > 500
        ORDER BY O.""DocDate"" DESC";
        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> BuOydagiBarchaSotuvlarAsyncAsync()
    {
        string sql = @"
        SELECT ""DocEntry"", ""DocNum"", ""CardCode"", ""CardName"", ""DocDate"", ""DocTotal""
        FROM ""OINV""
        WHERE ""DocDate"" >= '2025-05-01' AND ""DocDate"" <= '2025-05-31'";

        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> ShuOydaKelganPulniChiqarishAsync(DateTime fromDate, DateTime toDate)
    {
        string sql = $@"
        SELECT SUM(""DocTotal"") AS ""TotalReceived""
        FROM ""OINV""
        WHERE ""DocDate"" >= '{fromDate:yyyy-MM-dd}' AND ""DocDate"" <= '{toDate:yyyy-MM-dd}'";
        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> ShuOydaSotuvlarsoniniChiqarishAsync()
    {
        string sql = @"
        SELECT COUNT(*) AS ""SalesCount""
        FROM ""OINV""
        WHERE ""DocDate"" >= '2025-05-01' AND ""DocDate"" <= '2025-05-31'";
        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> GetSalesWithItemsAsync()
    {
        string sql = @"
        SELECT O.""DocNum"", I.""ItemCode"", I.""Dscription"", I.""Quantity"", I.""Price"", I.""LineTotal"" 
        FROM ""OINV"" O
        INNER JOIN ""INV1"" I ON O.""DocEntry"" = I.""DocEntry""";
        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> GetMostExpensiveSoldItemAsync()
    {
        string sql = @"
        SELECT TOP 1 ""ItemCode"", ""Dscription"", ""Price""
        FROM ""INV1""
        ORDER BY ""Price"" DESC";

        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> GetTop10MostExpensiveItemsAsync()
    {
        string sql = @"
        SELECT TOP 10 ""ItemCode"", ""Dscription"", ""Price""
        FROM ""INV1""
        ORDER BY ""Price"" DESC";

        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> GetMostExpensiveItemWithCustomerAsync()
    {
        string sql = @"
        SELECT TOP 1 I.""ItemCode"", I.""Dscription"", I.""Price"", C.""CardName""
        FROM ""INV1"" I
        INNER JOIN ""OINV"" O ON I.""DocEntry"" = O.""DocEntry""
        INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode""
        ORDER BY I.""Price"" DESC";

        return await ExecuteAllSqlAsync(sql);
    }

    public async Task<string> GetPurchasesWithLinesAsync()
    {
        string sql = @"
        SELECT P.""DocNum"", P.""CardCode"", P.""CardName"", L.""ItemCode"", L.""Dscription"", L.""Quantity"", L.""Price""
        FROM ""OPCH"" P
        INNER JOIN ""PCH1"" L ON P.""DocEntry"" = L.""DocEntry""";
        return await ExecuteAllSqlAsync(sql);
    }

    Task<string> ISQLInterfaces.ExecuteAllSqlAsync(string sqlText)
    {
        return ExecuteAllSqlAsync(sqlText);
    }
}






























































































//using System.Data.Common;
//using System.Text.Json;
//using System.Text;
//using BusinesssLogicLayer.Common;
//using BusinesssLogicLayer.Interfaces;
//using Newtonsoft.Json;
//using System.Text.RegularExpressions;

//namespace BusinesssLogicLayer.Services
//{
//    public class SQLServices : ISQLInterfaces
//    {
//        private readonly HttpClient _httpClient;
//        private readonly SapSession _sapSession;

//        public SQLServices(HttpClient httpClient,
//                            SapSession sapSession)
//        {
//            _httpClient = httpClient;
//            _sapSession = sapSession;
//        }

//        private void SetCookiesHeader()
//        {
//            _httpClient.DefaultRequestHeaders.Remove("Cookie");

//            var cookies = _sapSession.GetCookies();

//            // cache'dan olib kelinadi

//            if (!string.IsNullOrEmpty(cookies))
//            {
//                _httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
//            }
//        }
//        private async Task<string> HandleResponce(HttpResponseMessage responce)
//        {
//            var result = await responce.Content.ReadAsStringAsync();
//            if (!responce.IsSuccessStatusCode)
//            {
//                throw new Exception($"SAP error : {result}");
//            }

//            return result;
//        }


//        public async Task<string> UmumiyNarx500danKattaSotuvlarAsync()
//        {
//            SetCookiesHeader();

//            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$filter=DocTotal gt 500&$orderby=DocDate desc";

//            var response = await _httpClient.GetAsync(url);

//            return await HandleResponce(response);
//        }
//        public async Task<string> UmumiyNarx500danKattaMijozIsimlariBilanAsync()
//        {
//            SetCookiesHeader();

//            string url = @"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$filter=DocTotal gt 500&$expand=BusinessPartner&$select=DocNum,DocDate,DocTotal,BusinessPartner/CardName&$format=json";

//            var response = await _httpClient.GetAsync(url);
//            return await HandleResponce(response);
//        }
//        public async Task<string> BuOydagiBarchaSotuvlarAsyncAsync()
//        {
//            SetCookiesHeader();

//            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$filter=DocDate ge 2025-05-01 and DocDate le 2025-05-31";

//            var response = await _httpClient.GetAsync(url);

//            return await HandleResponce(response);
//        }
//        public async Task<string> ShuOydaKelganPulniChiqarishAsync(DateTime fromDate, DateTime toDate)
//        {
//            SetCookiesHeader();

//            var sqlQuery = new
//            {
//                SqlCode = "GetInvoiceTotals",
//                SqlName = "GetInvoiceTotals",
//                SqlText = $"SELECT \"DocTotal\" FROM \"OINV\" WHERE \"DocDate\" >= '{fromDate:yyyy-MM-dd}' AND \"DocDate\" <= '{toDate:yyyy-MM-dd}'"
//            };

//            var json = System.Text.Json.JsonSerializer.Serialize(sqlQuery, new JsonSerializerOptions
//            {
//                PropertyNamingPolicy = null
//            });

//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/SQLQueries";

//            var response = await _httpClient.PostAsync(url, content);

//            return await HandleResponce(response);
//        }
//        public async Task<string> ShuOydaSotuvlarsoniniChiqarishAsync()
//        {
//            SetCookiesHeader();

//            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices/$count?$filter=DocDate ge 2025-05-01 and DocDate le 2025-05-31";

//            var response = await _httpClient.GetAsync(url);

//            return await HandleResponce(response);
//        }


//        public async Task<string> ExecuteAllSqlAsync(string sqlText)
//        {
//            SetCookiesHeader();

//            var timestamp = DateTime.UtcNow.Ticks;
//            var queryCode = $"Query_{timestamp}";

//            var sqlQuery = new
//            {
//                SqlCode = queryCode,
//                SqlName = queryCode,
//                SqlText = sqlText
//            };

//            var json = System.Text.Json.JsonSerializer.Serialize(sqlQuery, new JsonSerializerOptions
//            {
//                PropertyNamingPolicy = null
//            });

//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            // STEP 1: POST – SQLQuery'ni yaratish
//            var postUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/SQLQueries";
//            var postResponse = await _httpClient.PostAsync(postUrl, content);

//            if (!postResponse.IsSuccessStatusCode)
//                return await postResponse.Content.ReadAsStringAsync(); // xatolikni qaytar

//            // STEP 2: GET – SQLQuery natijasini olish
//            var getUrl = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/SQLQueries('{queryCode}')/List";
//            var getResponse = await _httpClient.GetAsync(getUrl);

//            return await getResponse.Content.ReadAsStringAsync();
//        }

//        public async Task<string> GetSalesOver500Async()
//        {
//            string sql = "SELECT * FROM \"OINV\" WHERE \"DocTotal\" > 500";
//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetSalesOver500WithCustomerAsync()
//        {
//            string sql = @"SELECT O.""DocNum"", O.""DocTotal"", C.""CardName"" 
//                   FROM ""OINV"" O
//                   INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode"" 
//                   WHERE O.""DocTotal"" > 500";
//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetSalesThisMonthAsync()
//        {
//            string sql = @"
//            SELECT * FROM ""OINV""
//            WHERE ""DocDate"" >= '2025-05-01' AND ""DocDate"" <= '2025-05-31'";
//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetTotalReceivedThisMonthAsync()
//        {
//            string sql = @"
//            SELECT SUM(""DocTotal"") AS ""TotalReceived""
//            FROM ""OINV""
//            WHERE ""DocDate"" >= '2025-05-01' AND ""DocDate"" <= '2025-05-31'";
//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetSalesCountThisMonthAsync()
//        {
//            string sql = @"
//            SELECT COUNT(*) AS ""SalesCount""
//            FROM ""OINV""
//            WHERE ""DocDate"" >= '2025-05-01' AND ""DocDate"" <= '2025-05-31'";
//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetSalesWithItemsAsync()
//        {
//            string sql = @"
//        SELECT O.""DocNum"", I.""ItemCode"", I.""Dscription"", I.""Quantity"", I.""Price"", I.""LineTotal"" 
//        FROM ""OINV"" O
//        INNER JOIN ""INV1"" I ON O.""DocEntry"" = I.""DocEntry""";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetMostExpensiveSoldItemAsync()
//        {
//            string sql = @"
//        SELECT ""ItemCode"", ""Dscription"", ""Price""
//        FROM ""INV1""
//        ORDER BY ""Price"" DESC
//        LIMIT 1";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetTop10MostExpensiveItemsAsync()
//        {
//            string sql = @"
//        SELECT ""ItemCode"", ""Dscription"", ""Price""
//        FROM ""INV1""
//        ORDER BY ""Price"" DESC
//        LIMIT 10";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetMostExpensiveItemWithCustomerAsync()
//        {
//            string sql = @"
//        SELECT I.""ItemCode"", I.""Dscription"", I.""Price"", C.""CardName""
//        FROM ""INV1"" I
//        INNER JOIN ""OINV"" O ON I.""DocEntry"" = O.""DocEntry""
//        INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode""
//        ORDER BY I.""Price"" DESC
//        LIMIT 1";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetTop10FrequentCustomersAsync()
//        {
//            string sql = @"
//        SELECT C.""CardCode"", C.""CardName"", COUNT(*) AS ""PurchaseCount""
//        FROM ""OINV"" O
//        INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode""
//        GROUP BY C.""CardCode"", C.""CardName""
//        ORDER BY COUNT(*) DESC
//        LIMIT 10";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetTop10HighestSpendingCustomersAsync()
//        {
//            string sql = @"
//        SELECT C.""CardCode"", C.""CardName"", SUM(O.""DocTotal"") AS ""TotalSpent""
//        FROM ""OINV"" O
//        INNER JOIN ""OCRD"" C ON O.""CardCode"" = C.""CardCode""
//        GROUP BY C.""CardCode"", C.""CardName""
//        ORDER BY SUM(O.""DocTotal"") DESC
//        LIMIT 10";

//            return await ExecuteAllSqlAsync(sql);
//        }

//        public async Task<string> GetPurchasesWithLinesAsync()
//        {
//            string sql = @"
//        SELECT P.""DocNum"", P.""CardCode"", P.""CardName"", L.""ItemCode"", L.""Dscription"", L.""Quantity"", L.""Price""
//        FROM ""OPCH"" P
//        INNER JOIN ""PCH1"" L ON P.""DocEntry"" = L.""DocEntry""";

//            return await ExecuteAllSqlAsync(sql);
//        }
//    }
//}
































//using System.Data.Common;
//using System.Text.Json;
//using System.Text;
//using BusinesssLogicLayer.Common;
//using BusinesssLogicLayer.Interfaces;
//using Newtonsoft.Json;

//namespace BusinesssLogicLayer.Services
//{
//    public class SQLServices  : ISQLInterfaces
//    {
//        private readonly HttpClient _httpClient;
//        private readonly SapSession _sapSession;

//        public SQLServices(HttpClient httpClient, 
//                            SapSession sapSession)
//        {
//            _httpClient = httpClient;
//            _sapSession = sapSession;
//        }

//        private void SetCookiesHeader()
//        {
//            _httpClient.DefaultRequestHeaders.Remove("Cookie");

//            var cookies = _sapSession.GetCookies();

//            // cache'dan olib kelinadi

//            if (!string.IsNullOrEmpty(cookies))
//            {
//                _httpClient.DefaultRequestHeaders.Add("Cookie", cookies);
//            }
//        }
//        private async Task<string> HandleResponce(HttpResponseMessage responce)
//        {
//            var result = await responce.Content.ReadAsStringAsync();
//            if (!responce.IsSuccessStatusCode)
//            {
//                throw new Exception($"SAP error : {result}");
//            }

//            return result;
//        }


//        



//        public async Task<string> HarbittaSotuvniSotilganMollariBilanAsync()
//        {
//            SetCookiesHeader();

//            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$expand=DocumentLines";

//            var response = await _httpClient.GetAsync(url);

//            return await HandleResponce(response);
//        }

//        //public async Task<string> EngQimmatSotilganMolniChiqarishAsync()
//        //{
//        //    SetCookiesHeader();

//        //    var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/InvoiceLines?$orderby=Price desc&$top=1";

//        //    var response = await _httpClient.GetAsync(url);

//        //    return await HandleResponce(response);
//        //}
//        //public async Task<string> EngKopSotibOlgan10taMollarniChiqarishAsync()
//        //{
//        //    SetCookiesHeader();

//        //    string url = @"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$expand=DocumentLines,CardCodeDetails&$orderby=DocumentLines/Price desc&$top=1";

//        //    var response = await _httpClient.GetAsync(url);
//        //    return await HandleResponce(response);  
//        //}
//        //public async Task<string> EngQimmatSotiganMolMijozIsmiBilanAsync()
//        //{
//        //    throw new NotImplementedException();
//        //}
//        //public async Task<string> EngKopSotibOlganMijozlar10taAsync()
//        //{
//        //    SetCookiesHeader();

//        //    string url = @"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices?$select=CardCode,CardName&$top=1000";

//        //    var response = await _httpClient.GetAsync(url);
//        //    var jsonResponse = await HandleResponce(response);

//        //    var invoices = JsonConvert.DeserializeObject<InvoicesResponse>(jsonResponse);

//        //    var topCustomers = invoices.value
//        //        .GroupBy(inv => new { inv.CardCode, inv.CardName })
//        //        .Select(g => new {
//        //            CardCode = g.Key.CardCode,
//        //            CardName = g.Key.CardName,
//        //            PurchaseCount = g.Count()
//        //        })
//        //        .OrderByDescending(c => c.PurchaseCount)
//        //        .Take(10)
//        //        .ToList();

//        //    return JsonConvert.SerializeObject(topCustomers);
//        //}
//        //public async Task<string> EngKopPulSarflaganMijozlar10taAsync()
//        //{
//        //    throw new NotImplementedException();
//        //}
//        //public async Task<string> BarchaHaridlarVaHaridlarQatorlariBilanAsync()
//        //{
//        //    SetCookiesHeader();

//        //    // Xaridlar va xarid qatorlarini join qilib olish uchun SAP Service Layer so‘rovi
//        //    string url = @"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices?$expand=DocumentLines&$orderby=DocEntry,DocumentLines/LineNum";

//        //    var response = await _httpClient.GetAsync(url);
//        //    return await HandleResponce(response);
//        //}

//        //public Task<string> ShuOydaSotuvlarsoniniChiqarishAsync()
//        //{
//        //    throw new NotImplementedException();
//        //}





//    }
//}

