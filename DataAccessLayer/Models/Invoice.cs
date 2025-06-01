using System.Globalization;
using DTOs.InvoiceDtos;

namespace DataAccessLayer.Models
{
    public class Invoice
    {

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime? DocDate { get; set; }  
        public DateTime? DocDueDate { get; set; }
        public string DocCurrency { get; set; }
        public List<InvoiceLineDto> DocumentLines { get; set; }
    }
}
