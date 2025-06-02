using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.EmployeeDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ISapEmployeeService _sapEmployeeService;

        public EmployeeController(ISapEmployeeService sapEmployeeService)
        {
            _sapEmployeeService = sapEmployeeService;
        }

        [HttpGet("employee")]
        public async Task<IActionResult> GetEmployees(
                                                    [FromQuery] string? firstName = null,
                                                    [FromQuery] string? phoneNumber = null,
                                                    [FromQuery] string? homeAddress = null,
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10)

        {
            try
            {
                var result = await _sapEmployeeService.GetEmployeeAsync(firstName, phoneNumber, homeAddress, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xodimlar ro'yxatini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            try
            {
                var result = await _sapEmployeeService.PostEmployeeAsync(employee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xodim qo'shishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var result = await _sapEmployeeService.PatchEmployeeAsync(employeeId, updateEmployeeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xodimni yangilashda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            try
            {
                var result = await _sapEmployeeService.DeleteEmployeeAsync(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xodimni o‘chirishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            try
            {
                var result = await _sapEmployeeService.GetEmployeeByIdAsync(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xodimni ID orqali olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}
