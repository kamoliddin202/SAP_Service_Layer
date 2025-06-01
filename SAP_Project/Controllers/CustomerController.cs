using BusinesssLogicLayer.Interfaces;
using BusinesssLogicLayer.Services;
using DataAccessLayer.Models;
using DTOs.CutomerDtos;
using DTOs.SupplierDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customersService;

        public CustomerController(ICustomerService customerService)
        {
            _customersService = customerService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCustomers([FromQuery] string? cardCode, 
                                                      [FromQuery] string? cardName, 
                                                      [FromQuery] int page = 1, 
                                                      [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _customersService.GetCustomersAsync(cardCode, cardName, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijozlar ro'yxatini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostCustomer([FromBody] Customer model)
        {
            try
            {
                var result = await _customersService.PostCustomerAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijoz yaratishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPatch("{cardCode}")]
        public async Task<IActionResult> PatchCustomer(string cardCode, [FromBody] UpdateCustomerDto dto)
        {
            try
            {
                var result = await _customersService.PatchCustomerAsync(cardCode, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijozni yangilashda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpDelete("{cardCode}")]
        public async Task<IActionResult> DeleteCustomer(string cardCode)
        {
            try
            {
                var result = await _customersService.DeleteCustomerAsync(cardCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijozni o‘chirishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("ById/{customerId}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            try
            {
                var result = await _customersService.GetCustomerByIdAsync(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijozni ID orqali olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("withinvoice/{cardCode}", Name = "GetCustomerWithInvoice")]
        public async Task<IActionResult> GetCustomerWithInvoice(string cardCode)
        {
            try
            {
                var result = await _customersService.GetCustemerWithInvoices(cardCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Mijoz va uning fakturalarini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}
