namespace DataAccessLayer.Models
{
    public class PurchesInvoiceOrderLine
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string WarehouseCode { get; set; }
        public int UoMEntry { get; set; }
        public string TaxCode { get; set; }   
    }
}
