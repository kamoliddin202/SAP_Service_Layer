using DataAccessLayer.Models;
using DTOs.CutomerDtos;
using DTOs.SupplierDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface ICustomerService
    {
        Task<string> GetCustomersAsync(string? cardCode, string? cardName, int page = 1, int pageSize = 10);
        Task<string> PostCustomerAsync(Customer model);
        Task<string> PatchCustomerAsync(string cardCode, UpdateCustomerDto dto);
        Task<string> DeleteCustomerAsync(string cardCode);
        Task<string> GetCustomerByIdAsync(string  customerId);   
        Task<string> GetCustemerWithInvoices(string cardCode);
    }
}
