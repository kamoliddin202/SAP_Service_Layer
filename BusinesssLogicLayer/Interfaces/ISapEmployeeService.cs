using DataAccessLayer.Models;
using DTOs.EmployeeDto;

namespace BusinesssLogicLayer.Interfaces
{
    public interface ISapEmployeeService
    {
        Task<string> GetEmployeeAsync(string? firstName = null, string? lastName = null, string? department = null, int page = 1, int pageSize = 10);
        Task<string> PostEmployeeAsync(Employee body);
        Task<string> PatchEmployeeAsync(int employeeId, UpdateEmployeeDto updateBody);
        Task<string> DeleteEmployeeAsync(int employeeId);
        Task<string> GetEmployeeByIdAsync(int employeeId);
    }
}
