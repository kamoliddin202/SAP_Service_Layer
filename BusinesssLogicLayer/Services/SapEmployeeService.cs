using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.EmployeeDto;

namespace BusinesssLogicLayer.Services
{
    public class    SapEmployeeService : ISapEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly SapSession _sapSession;

        public SapEmployeeService(HttpClient httpEmployee,
                            SapSession sapSession)
        {
            _httpClient = httpEmployee;
            _sapSession = sapSession;
        }

        public async Task<string> DeleteEmployeeAsync(int employeeId)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/EmployeesInfo({employeeId})";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse(response);
        }

        public async Task<string> GetEmployeeAsync(string? firstName = null, string? lastName = null, string? department = null, int page = 1, int pageSize = 10)
        {
            SetCookiesHeader();

            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(firstName))
                filters.Add($"contains(FirstName, '{firstName}')");

            if (!string.IsNullOrWhiteSpace(lastName))
                filters.Add($"contains(LastName, '{lastName}')");

            if (!string.IsNullOrWhiteSpace(department))
                filters.Add($"Department eq {department}");

            string filterQuery = filters.Count > 0 ? $"$filter={string.Join(" and ", filters)}&" : "";

            int skip = (page - 1) * pageSize;
            string pagingQuery = $"$top={pageSize}&$skip={skip}";

            string url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/EmployeesInfo?{filterQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse(response);
        }

        public async Task<string> PatchEmployeeAsync(int employeeId, UpdateEmployeeDto updateDto)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/EmployeesInfo({employeeId})";

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(updateDto.FirstName))
                payload["FirstName"] = updateDto.FirstName;

            if (!string.IsNullOrWhiteSpace(updateDto.LastName))
                payload["LastName"] = updateDto.LastName;

            if (!string.IsNullOrWhiteSpace(updateDto.JobTitle))
                payload["JobTitle"] = updateDto.JobTitle;

            if (!string.IsNullOrWhiteSpace(updateDto.Remarks))
                payload["Remarks"] = updateDto.Remarks;

            if (!string.IsNullOrWhiteSpace(updateDto.WorkCountryCode))
                payload["WorkCountryCode"] = updateDto.WorkCountryCode;

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


        public async Task<string> PostEmployeeAsync(Employee employee)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/EmployeesInfo";

            var json = JsonSerializer.Serialize(employee, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = null 
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return await HandleResponse(response);
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
        private async Task<string> HandleResponse(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"SAP error : {result}");
            }

            return result;
        }

        public async Task<string> GetEmployeeByIdAsync(int employeeId)
        {
            SetCookiesHeader();

            var url = $"https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/EmployeesInfo({employeeId})";

            var result = await _httpClient.GetAsync(url);
            return await HandleResponse(result);
        }
    }
}
