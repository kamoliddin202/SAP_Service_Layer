using DataAccessLayer.Models;
using DTOs.UpdateItemDtos;
using DTOs.SupplierDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface ISupplierService
    {
        Task<string> PostSupplierAsync(Supplier supplier);
        Task<string> GetSuppliersAsync(string? cardCode, string? cardName, int page = 1, int pageSize = 10);
        Task<string> DeleteSupplierAsync(string cardCode);
        Task<string> PatchSupplierAsync(string cardCode, UpdateSupplierDto dto); 
        Task<string> GetSuplierByIdAsync(string supplierId);    
    }
}
