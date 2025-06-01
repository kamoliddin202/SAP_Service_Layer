using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.PurchaseOrderModels;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseInvoiceController : ControllerBase
    {
        private readonly IPurchaseInvoiceService _purchaseService;

        public PurchaseInvoiceController(IPurchaseInvoiceService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetPurchases(
                                                    [FromQuery] string? cardCode,
                                                    [FromQuery] string? docNum,
                                                    [FromQuery] DateTime? fromDate,
                                                    [FromQuery] DateTime? toDate,
                                                    [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _purchaseService.GetPurchaseInvoiceAsync(cardCode, docNum, fromDate, toDate, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Xarid fakturalarini olishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> CreatePurchase(PurchaseInvoice purchaseDto)
        {
            try
            {
                var result = await _purchaseService.PostPurchaseInvoiceAsync(purchaseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Xarid fakturasini yaratishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpPatch("{docEntry}")]
        public async Task<IActionResult> UpdatePurchase(int docEntry, UpdatePurchaseInvoiceDto updateDto)
        {
            try
            {
                var result = await _purchaseService.PatchPurchaseInvoiceAsync(docEntry, updateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Xarid fakturasini yangilashda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{docEntry:int}")]
        public async Task<IActionResult> DeletePurchase(int docEntry)
        {
            try
            {
                var result = await _purchaseService.DeletePurchaseInvoiceAsync(docEntry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Xarid fakturasini o'chirishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        //[HttpGet("{cardCode}")]
        //public async Task<IActionResult> GetPurchaseByIdAsync(int cardCode)
        //{
        //    try
        //    {
        //        var result = await _purchaseService.GetPurchaseInvoiceByIdAsync(cardCode);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            Message = "Xarid fakturasini olishda xatolik yuz berdi!",
        //            Error = ex.Message
        //        });
        //    }
        //}
    }
}
