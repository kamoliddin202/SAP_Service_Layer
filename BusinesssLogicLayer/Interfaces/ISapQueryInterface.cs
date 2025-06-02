namespace BusinesssLogicLayer.Interfaces
{
    public interface ISapQueryInterface
    {
        Task<string> GetInvoicesAsync(string? cardCode, string? docDateFrom, string? docDateTo, int page = 1, int pageSize = 10);
        Task<string> GetInvoiceDetailsAsync(int docEntry);
        Task<string> GetMonthlyPaymentsAsync(string cardCode, int year, int month);
        Task<string> GetCustomersAsync(string? cardName);
        Task<string> GetPaymentsAsync(int docEntry);
        Task<string> GetPurchaseOrdersAsync(string? docNum, string? vendorName);
        Task<string> GetPurchaseOrderDetailsAsync(string docEntry);
        Task<string> GetItemsAsync(string? itemCode, string? itemName);
    }
}
