using BusinesssLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLController : ControllerBase
    {
        private readonly ISQLInterfaces _sqlService;

        public SQLController(ISQLInterfaces sQLInterfaces)
        {
            _sqlService = sQLInterfaces;
        }

        [HttpGet("sales-over-500")]
        public async Task<IActionResult> GetSalesOver500()
        {
            try
            {
                var result = await _sqlService.UmumiyNarx500danKattaSotuvlarAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "500 dan katta sotuvlarni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("sales-over-500-with-customer")]
        public async Task<IActionResult> GetSalesOver500WithCustomer()
        {
            try
            {
                var result = await _sqlService.UmumiyNarx500danKattaMijozIsimlariBilanAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "500 dan katta sotuvlarni mijozlar bilan olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("sales-this-month")]
        public async Task<IActionResult> GetSalesThisMonth()
        {
            try
            {
                var result = await _sqlService.BuOydagiBarchaSotuvlarAsyncAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Bu oydagi sotuvlarni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("total-received-this-month")]
        public async Task<IActionResult> GetTotalReceivedThisMonth([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            try
            {
                var result = await _sqlService.ShuOydaKelganPulniChiqarishAsync(fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Bu oydagi kelgan pulni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("sales-count-this-month")]
        public async Task<IActionResult> GetSalesCountThisMonth()
        {
            try
            {
                var result = await _sqlService.BuOydagiBarchaSotuvlarAsyncAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Bu oydagi sotuvlar sonini olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("sales-with-items")]
        public async Task<IActionResult> GetSalesWithItems()
        {
            try
            {
                var result = await _sqlService.GetSalesWithItemsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Sotuvlarni buyumlari bilan olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("most-expensive-sold-item")]
        public async Task<IActionResult> GetMostExpensiveSoldItem()
        {
            try
            {
                var result = await _sqlService.GetMostExpensiveSoldItemAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Eng qimmat sotilgan buyumni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("top-10-expensive-items")]
        public async Task<IActionResult> GetTop10MostExpensiveItems()
        {
            try
            {
                var result = await _sqlService.GetTop10MostExpensiveItemsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Eng qimmat 10 ta buyumni olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("most-expensive-item-with-customer")]
        public async Task<IActionResult> GetMostExpensiveItemWithCustomer()
        {
            try
            {
                var result = await _sqlService.GetMostExpensiveItemWithCustomerAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Eng qimmat buyumni mijoz bilan olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }

        [HttpGet("purchases-with-lines")]
        public async Task<IActionResult> GetPurchasesWithLines()
        {
            try
            {
                var result = await _sqlService.GetPurchasesWithLinesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Xaridlarni buyumlar bilan olishda xatolik yuz berdi.", Error = ex.Message });
            }
        }
    }
}








