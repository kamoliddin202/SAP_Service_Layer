namespace DataAccessLayer.Models
{
    public class IncomingPayment
    {
        public string CardCode { get; set; } // Mijoz kodi                  yaratgan mijozim bilan bog'lanadi
        public string DocDate { get; set; } // Hujjat sanasi (yyyy-MM-dd)   oddiy sana
        public string CashAccount { get; set; } // Kassa hisob raqami       ChartsOfAccountdagi AcctCode
        public decimal CashSum { get; set; } // Naqd pul miqdori            Naqt pul 
        public string DocType { get; set; } //                              Hujjat turi ("rCustomer" = mijozdan to'lov) 
        public List<InvoiceLine> InvoiceLines { get; set; }                 // Invoice'lar bo'yicha to'lovlar
    }
}

public class InvoiceLine
{
    public int InvoiceDocEntry { get; set; } // Invoice jujjatining unikal identificatori yani docEntry
    public decimal SumApplied { get; set; } // Su invoise uchun to'langan summa 
}

