namespace DTOs.IncomingPaymentsDtos
{
    public class UpdateIncomingPaymentDto
    {
        public string DocDate { get; set; } // Hujjat sanasi (yyyy-MM-dd)   oddiy sana
        public decimal CashSum { get; set; } // Naqd pul miqdori            Naqt pul 
        public string DocType { get; set; } //                              Hujjat turi ("rCustomer" = mijozdan to'lov) 
    }
}
