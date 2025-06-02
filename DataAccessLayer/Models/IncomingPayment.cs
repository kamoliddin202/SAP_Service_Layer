namespace DataAccessLayer.Models
{
    public class IncomingPayment
    {
        public string CardCode { get; set; } // Mijoz kodi                  yaratgan mijozim bilan bog'lanadi
        public string DocDate { get; set; } // Hujjat sanasi (yyyy-MM-dd)   oddiy sana
        public decimal CashSum { get; set; } // Naqd pul miqdori            Naqt pul 
        public string DocType { get; set; } //                              Hujjat turi ("rCustomer" = mijozdan to'lov) 
        public List<PaymentInvoice> PaymentInvoices { get; set; } = new();
    }

    public class PaymentInvoice
    {
        public string InvoiceType { get; set; }       // Misol: "it_Invoice"
        public int DocEntry { get; set; }             // Fakturaning ID raqami
        public decimal SumApplied { get; set; }       // Qancha pul ajratildi
    }

}






/*
 
{
  "CardCode": "Customer_1",            // Mijoz kodi
  "DocDate": "2025-06-01",             // To‘lov sanasi
  "CashSum": 1.75,                     // Naqd pul orqali olingan summa
  "DocType": "rCustomer",              // Mijoz to‘lovi
  "InvoiceLines": [                    // Qaysi fakturaga to‘lov bog‘lanadi
    {
      "InvoiceType": "invInvoices",    // Faktura turi
      "DocEntry": 3934,                // Faktura ID (faktura raqami emas, SAPdagi ichki ID)
      "SumApplied": 1.75               // To‘langan summa
    }
  ]
}
 
 
 
 
 */