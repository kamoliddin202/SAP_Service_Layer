using DataAccessLayer.Models;
using DTOs.PurchaseOrderModels;

namespace BusinesssLogicLayer.Interfaces
{
    public interface IPurchaseInvoiceService
    {
        Task<string> GetPurchaseInvoiceAsync(string? cardCode, string? docNum, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10);
        Task<string> PostPurchaseInvoiceAsync(PurchaseInvoice body);
        Task<string> PatchPurchaseInvoiceAsync(int docEntiry, UpdatePurchaseInvoiceDto updateBody);
        Task<string> DeletePurchaseInvoiceAsync(int docEntiry);
        Task<string> GetPurchaseInvoiceByIdAsync(int cardCode);
    }
}
