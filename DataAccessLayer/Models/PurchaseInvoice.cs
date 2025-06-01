using System.Text.Json.Serialization;

namespace DataAccessLayer.Models
{
    public class PurchaseInvoice
    {
        [JsonPropertyName("CardCode")]
        public string CardCode { get; set; } 
        public DateTime DocDate { get; set; }        
        public DateTime? DocDueDate { get; set; }    
        public List<PurchesInvoiceOrderLine> DocumentLines { get; set; }

    }
}
