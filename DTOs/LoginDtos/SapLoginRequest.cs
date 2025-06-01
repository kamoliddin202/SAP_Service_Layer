using System.Text.Json.Serialization;

namespace DTOs.LoginDtos
{
    public class SapLoginRequest
    {
        [JsonPropertyName("CompanyDB")]
        public string CompanyDb { get; set; }
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
