using System.Text.Json.Serialization;

namespace DataAccessLayer.Models
{
    public class Employee
    {
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string JobTitle { get; set; }    
        public string Remarks { get; set; }
        public string WorkCountryCode { get; set; }  
    }

}
