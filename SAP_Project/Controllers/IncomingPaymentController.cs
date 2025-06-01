using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.IncomingPaymentsDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomingPaymentController : ControllerBase
    {
        private readonly IIncomingPaymentService _incomingPaymentService;

        public IncomingPaymentController(IIncomingPaymentService incomingPaymentService)
        {
            _incomingPaymentService = incomingPaymentService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetInComingPayments(
                                                            [FromQuery] string? cardCode,
                                                            [FromQuery] string? docNum,
                                                            [FromQuery] DateTime? fromDate,
                                                            [FromQuery] DateTime? toDate,
                                                            [FromQuery] int page = 1,
                                                            [FromQuery] int pageSize = 10   )
        {
            try
            {
                var result = await _incomingPaymentService.GetIncomingPaymentAsync(cardCode, docNum, fromDate, toDate, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "To‘lovlar ro'yxatini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }


        [HttpPatch("put")]
        public async Task<IActionResult> Patch(int docEntiry, UpdateIncomingPaymentDto updateIncomingPaymentDto)
        {
            try
            {
                var result = await _incomingPaymentService.PatchIncomingPaymentAsync(docEntiry, updateIncomingPaymentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "To‘lovni yangilashda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int docEntiry)
        {
            try
            {
                var result = await _incomingPaymentService.DeleteIncomingPaymentAsync(docEntiry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "To‘lovni o‘chirishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post(IncomingPayment incomingPayment)
        {
            try
            {
                var result = await _incomingPaymentService.PostIncomingPaymentAsync(incomingPayment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "To‘lovni qo‘shishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("ById/{cardCode}")]
        public async Task<IActionResult> GetIncoimingPayementById(string cardCode)
        {
            try
            {
                var result = await _incomingPaymentService.GetIncomingPaymentsByIdAsync(cardCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "To‘lovni qo‘shishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}
