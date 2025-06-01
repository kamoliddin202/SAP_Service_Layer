namespace DataAccessLayer.Models
{
    public class Customer
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; } = "C"; 
        public string Currency { get; set; }
        public string Phone1 { get; set; }
        public string EmailAddress { get; set; }
    }
}
