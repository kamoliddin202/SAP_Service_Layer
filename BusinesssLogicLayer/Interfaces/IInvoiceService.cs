using DataAccessLayer.Models;
using DTOs.EmployeeDto;
using DTOs.InvoiceDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface IInvoiceService
    {
        Task<string> GetInvoicesAsync(string? cardCode, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 1);
        Task<string> PostInvoicesAsync(Invoice invoice);
        Task<string> PatchInvoicesAsync(int docEntry, UpdateInvoiceDto updateInvoiceDto);
        Task<string> CancelInvoiceAsync(int docEntry);
        Task<string> GetInvoiceByIdAsync(int docEntry);
        Task<string> GetInvoiceExpandIncomingPayments(int    cardCode);
    }
}
