using DataAccessLayer.Interfaces;
using DTOs.LoginDtos;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace DataAccessLayer.Repasitories
{
    public class SapAuthServiceRepasitory : ISapAuthServiceInterface
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        // Cache kalitlari
        private const string B1SessionKey = "Sap_B1Session_Cookie";
        private const string RouteIdKey = "Sap_RouteId_Cookie";

        public SapAuthServiceRepasitory(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<string> LoginAsync(SapLoginRequest request)
        {
            var url = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Login";

            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadAsStringAsync();
                throw new Exception($"SAP login failed: {errors}");
            }

            if (!response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                throw new Exception("Set-Cookie header not found in SAP response.");
            }

            // Cookie'larni ajratish
            var b1SessionCookie = cookieHeaders.FirstOrDefault(c => c.Contains("B1SESSION"));
            var routeIdCookie = cookieHeaders.FirstOrDefault(c => c.Contains("ROUTEID"));

            if (string.IsNullOrEmpty(b1SessionCookie))
                throw new Exception("SAP session cookie (B1SESSION) not found!");

            // Cookie'larni cache ga saqlash - 30 daqiqa davomida
            _memoryCache.Set(B1SessionKey, b1SessionCookie, TimeSpan.FromMinutes(30));

            if (!string.IsNullOrEmpty(routeIdCookie))
            {
                _memoryCache.Set(RouteIdKey, routeIdCookie, TimeSpan.FromMinutes(30));
            }

            return b1SessionCookie;
        }

        // Cache'dan B1SESSION ni olish
        public string? GetCachedB1Session()
        {
            _memoryCache.TryGetValue(B1SessionKey, out string session);
            return session;
        }

        // Cache'dan ROUTEID ni olish
        public string? GetCachedRouteId()
        {
            _memoryCache.TryGetValue(RouteIdKey, out string route);
            return route;
        }
    }
}


/*
 cookies ichidan B1SESSION nomli cookie ni topadi. Bu SAP sessiya identifikatori bo‘lib, 
keyingi so‘rovlarda foydalaniladi (autentifikatsiya maqsadida). 0
 */
