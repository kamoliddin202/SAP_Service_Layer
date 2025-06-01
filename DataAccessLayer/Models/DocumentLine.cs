namespace DataAccessLayer.Models
{
    public class DocumentLine
    {
        public string ItemCode { get; set; } = default!; 
        public double Quantity { get; set; }
        public double Price { get; set; } 
    }
}
