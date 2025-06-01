using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.SupplierDtos;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostSupplier( Supplier supplier)
        {
            try
            {
                var result = await _supplierService.PostSupplierAsync(supplier);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Yangi supplier qo'shishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetSuppliers([FromQuery] string? cardCode, 
                                                      [FromQuery] string? cardName, 
                                                      [FromQuery] int page = 1, 
                                                      [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _supplierService.GetSuppliersAsync(cardCode, cardName, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Supplierlarni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpDelete("{cardCode}")]
        public async Task<IActionResult> DeleteSupplier(string cardCode)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(cardCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Supplierni o'chirishda xatolik yuz berdi: {cardCode}", Error = ex.Message });
            }
        }

        [HttpPatch("{cardCode}")]
        public async Task<IActionResult> PatchSupplier([FromRoute] string cardCode, [FromBody] UpdateSupplierDto dto)
        {
            try
            {
                var result = await _supplierService.PatchSupplierAsync(cardCode, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Supplierni yangilashda xatolik yuz berdi: {cardCode}", Error = ex.Message });
            }
        }

        [HttpGet("{supplierId}")]
        public async Task<IActionResult> GetSuplierById([FromRoute] string supplierId)
        {
            try
            {
                var result = await _supplierService.GetSuplierByIdAsync(supplierId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Supplierni ID bo'yicha olishda xatolik yuz berdi: {supplierId}", Error = ex.Message });
            }
        }
    }
}
