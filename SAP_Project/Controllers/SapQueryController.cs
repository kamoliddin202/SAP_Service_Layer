using BusinesssLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapQueryController : ControllerBase
    {
        private readonly ISapQueryInterface _sapQueryInterface;

        public SapQueryController(ISapQueryInterface sapQueryInterface)
        {
            _sapQueryInterface = sapQueryInterface;
        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetInvoices(string? cardCode, string? docDateFrom, string? docDateTo, int page = 1, int pageSize = 10)
        {
            var result = await _sapQueryInterface.GetInvoicesAsync(cardCode, docDateFrom, docDateTo, page, pageSize);
            return Ok(result);
        }

        [HttpGet("invoices/{docEntry}/details")]
        public async Task<IActionResult> GetInvoiceDetails(int docEntry)
        {
            var result = await _sapQueryInterface.GetInvoiceDetailsAsync(docEntry);
            return Ok(result);
        }

        [HttpGet("monthlypayments")]
        public async Task<IActionResult> GetMonthlyPayments(string cardCode, int year, int month)
        {
            var result = await _sapQueryInterface.GetMonthlyPaymentsAsync(cardCode, year, month);
            return Ok(result);
        }

        [HttpGet("get/customers")]
        public async Task<IActionResult> GetCustomers(string? cardName)
        {
            var result = await _sapQueryInterface.GetCustomersAsync(cardName);
            return Ok(result);
        }

        [HttpGet("payments/{docEntry}")]
        public async Task<IActionResult> GetPayments(int docEntry)
        {
            var result = await _sapQueryInterface.GetPaymentsAsync(docEntry);
            return Ok(result);
        }


        [HttpGet("purchaseorders")]
        public async Task<IActionResult> GetPurchaseOrders(string? docNum, string? vendorName)
        {
            var result = await _sapQueryInterface.GetPurchaseOrdersAsync(docNum, vendorName);
            return Ok(result);
        }

        [HttpGet("purchaseorders/{docEntry}/details")]
        public async Task<IActionResult> GetPurchaseOrderDetails(string docEntry)
        {
            var result = await _sapQueryInterface.GetPurchaseOrderDetailsAsync(docEntry);
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems(string? itemCode, string? itemName)
        {
            var result = await _sapQueryInterface.GetItemsAsync(itemCode, itemName);
            return Ok(result);
        }
    }
}
