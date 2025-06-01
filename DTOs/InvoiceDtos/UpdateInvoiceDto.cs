namespace DTOs.InvoiceDtos
{
    public class UpdateInvoiceDto
    {
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string DocCurrency { get; set; }
    }
}
