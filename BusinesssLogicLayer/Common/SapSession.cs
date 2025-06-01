using Microsoft.Extensions.Caching.Memory;

namespace BusinesssLogicLayer.Common
{
    public class SapSession
    {
        private readonly IMemoryCache _memoryCache;
        private const string B1SessionKey = "Sap_B1Session_Cookie";
        private const string RouteIdKey = "Sap_RouteId_Cookie";

        public SapSession(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GetCookies()
        {
            _memoryCache.TryGetValue(B1SessionKey, out string b1Session);
            _memoryCache.TryGetValue(RouteIdKey, out string routeId);

            var cookies = new List<string>();

            if (!string.IsNullOrWhiteSpace(b1Session))
                cookies.Add(b1Session);

            if (!string.IsNullOrWhiteSpace(routeId))
                cookies.Add(routeId);

            return string.Join("; ", cookies);
        }
    }
}
