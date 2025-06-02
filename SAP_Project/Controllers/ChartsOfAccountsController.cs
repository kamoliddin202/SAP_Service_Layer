using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.ChartOfAccountDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsOfAccountsController : ControllerBase
    {
        private readonly IChartOfAccountService _accountService;

        public ChartsOfAccountsController(IChartOfAccountService chartOfAccountService)
        {
            _accountService = chartOfAccountService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAccounts(
                                                    [FromQuery] string? acctCode,
                                                    [FromQuery] string? acctName,
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _accountService.GetChartOfAccountsAsync(acctCode, acctName, page = 1, pageSize = 100);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Hisoblar ro'yxatini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] ChartOfAccounts accountDto)
        {
            try
            {
                var result = await _accountService.PostChartOfAccountsAsync(accountDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Hisob yaratishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPatch("{acctCode}")]
        public async Task<IActionResult> Update(string acctCode, UpdateChartOfAccountDto updateDto)
        {
            try
            {
                var result = await _accountService.PatchChartOfAccountsAsync(acctCode, updateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Hisobni yangilashda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpDelete("{acctCode}")]
        public async Task<IActionResult> DeleteAccount(string acctCode)
        {
            try
            {
                var result = await _accountService.DeleteChartOfAccountsAsync(acctCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Hisobni o‘chirishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetChartsOfAccountsById(string code)
        {
            try
            {
                var result = await _accountService.GetChartsOfAccountsByIdAsync(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Hisob ma'lumotlarini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}
