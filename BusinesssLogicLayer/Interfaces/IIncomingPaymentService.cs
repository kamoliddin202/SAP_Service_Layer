using DataAccessLayer.Models;
using DTOs.IncomingPaymentsDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface IIncomingPaymentService
    {
        Task<string> GetIncomingPaymentAsync(string? cardCode, string? docNum, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        Task<string> PostIncomingPaymentAsync(IncomingPayment body);
        Task<string> PatchIncomingPaymentAsync(int docEntry, UpdateIncomingPaymentDto updateBody);
        Task<string> DeleteIncomingPaymentAsync(int docEntry);
        Task<string> GetIncomingPaymentsByIdAsync(string cardCode);  
    }
}
