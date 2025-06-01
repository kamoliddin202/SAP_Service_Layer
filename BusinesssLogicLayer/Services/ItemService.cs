using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DTOs.UpdateItemDtos;

namespace BusinesssLogicLayer.Services
{
    public class ItemService : IItemsService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public ItemService(HttpClient httpClient, 
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

        public async Task<string> GetItemsAsync(string? itemCode, string? itemName, string? foreignName, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(itemCode))
                filters.Add($"contains(ItemCode, '{itemCode}')");

            if (!string.IsNullOrWhiteSpace(itemName))
                filters.Add($"contains(ItemName, '{itemName}')");

            if (!string.IsNullOrWhiteSpace(foreignName))
                filters.Add($"contains(ForeignName, '{foreignName}')");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponce(response);

        }

        public async Task<string> PostItemAsync(Item body)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items";

            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // bu eng MUHIM QISM
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);


            return await HandleResponce(response);

        }

        public async Task<string> DeleteItemAsync(string ItemCode)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items('{ItemCode}')";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);

            return await HandleResponce(response);
        }

        public async Task<string> PatchItemAsync(string itemCode, UpdateItemDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items('{itemCode}')";

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(updateDto.ItemName))
                payload["ItemName"] = updateDto.ItemName;

            if (!string.IsNullOrEmpty(updateDto.ItemType))
                payload["ItemType"] = updateDto.ItemType;

            if (!string.IsNullOrEmpty(updateDto.SalesItem))
                payload["SalesItem"] = updateDto.SalesItem;

            if (!string.IsNullOrEmpty(updateDto.InventoryItem))
                payload["InventoryItem"] = updateDto.InventoryItem;

            if (!string.IsNullOrEmpty(updateDto.PurchaseItem))
                payload["PurchaseItem"] = updateDto.PurchaseItem;

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

            return await HandleResponce(response);
        }

        public async Task<string> GetItemByIdAsync(string ItemCode) 
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Items('{ItemCode}')";

            var response = await _httpClient.GetAsync(url);

            return await HandleResponce(response); ;
        }
    }
}
