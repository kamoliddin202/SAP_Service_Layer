using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.InvoiceDtos;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetInvoices(
                                                    [FromQuery] string? cardCode,
                                                    [FromQuery] DateTime? fromDate,
                                                    [FromQuery] DateTime? toDate,
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _invoiceService.GetInvoicesAsync(cardCode, fromDate, toDate, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Fakturalar ro'yxatini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> CreateInvoice(Invoice invoice)
        {
            try
            {
                var result = await _invoiceService.PostInvoicesAsync(invoice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Faktura yaratishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPatch("{docEntry}")]
        public async Task<IActionResult> UpdateInvoice(int docEntry, [FromBody] UpdateInvoiceDto dto)
        {
            try
            {
                var result = await _invoiceService.PatchInvoicesAsync(docEntry, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Fakturani yangilashda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("cancel/{docEntry}")]
        public async Task<IActionResult> CancelInvoice(int docEntry)
        {
            try
            {
                var result = await _invoiceService.CancelInvoiceAsync(docEntry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Fakturani bekor qilishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("{cardCode}")]
        public async Task<IActionResult> GetInoiceById(int cardCode)
        {
            try
            {
                var result = await _invoiceService.GetInvoiceByIdAsync(cardCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Faktura topilmadi yoki olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("withincomingpayments/{cardcode}")]
        public async Task<IActionResult> GetInvoiceWithIncomingPayments(int cardcode)
        {
            try
            {
                var result = await _invoiceService.GetInvoiceExpandIncomingPayments(cardcode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Faktura topilmadi yoki olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }

}
