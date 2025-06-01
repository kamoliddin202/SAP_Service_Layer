namespace DataAccessLayer.Models
{
    public class Supplier
    {
        public string CardCode { get; set; } 
        public string CardName { get; set; } 
        public string CardType { get; set; } = "S";
        public string Currency { get; set; }  
        public string Phone1 { get; set; }   
        public string EmailAddress { get; set; }  
    }
}
