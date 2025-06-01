using DataAccessLayer.Models;
using DTOs.ChartOfAccountDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface IChartOfAccountService
    {
        Task<string> GetChartOfAccountsAsync(string? acctCode, string? acctName, int page = 1, int pageSize = 1);
        Task<string> PostChartOfAccountsAsync(ChartOfAccounts body);
        Task<string> PatchChartOfAccountsAsync(string accode, UpdateChartOfAccountDto updateBody);
        Task<string> DeleteChartOfAccountsAsync(string accode);
        Task<string> GetChartsOfAccountsByIdAsync(string code);     
    }
}
