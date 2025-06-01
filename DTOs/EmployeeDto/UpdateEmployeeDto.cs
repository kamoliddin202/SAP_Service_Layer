using System.Text.Json.Serialization;

namespace DTOs.EmployeeDto
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Remarks { get; set; }
        public string WorkCountryCode { get; set; }
    }
}
