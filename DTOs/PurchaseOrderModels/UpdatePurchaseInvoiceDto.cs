using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DTOs.PurchaseOrderModels
{
    public class UpdatePurchaseInvoiceDto
    {
        public DateTime DocDate { get; set; }
        public DateTime?  DocDueDate { get; set; }
    }
}
