namespace DataAccessLayer.Models
{
    public class InvoiceLineDto
    {
        public string ItemCode { get; set; }  
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int UoMEntry { get; set; }
        public string WarehouseCode { get; set; } 
    }
}
