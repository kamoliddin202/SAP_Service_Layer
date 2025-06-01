using BusinesssLogicLayer.Interfaces;
using DataAccessLayer.Models;
using DTOs.UpdateItemDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemsService _itemsService;

        public ItemController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetItems(
                                                [FromQuery] string? itemCode,
                                                [FromQuery] string? itemName,
                                                [FromQuery] string? foreignName,
                                                [FromQuery] int page = 1,
                                                [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _itemsService.GetItemsAsync(itemCode, itemName, foreignName, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Mahsulotlar ro'yxatini olishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }


        [HttpPatch("{itemCode}")]
        public async Task<IActionResult> UpdateItem(string itemCode, UpdateItemDto updateItemDto)
        {
            try
            {
                var result = await _itemsService.PatchItemAsync(itemCode, updateItemDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Mahsulotni yangilashda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostItem(Item item)
        {
            try
            {
                var result = await _itemsService.PostItemAsync(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Mahsulotni yaratishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{itemCode}")]
        public async Task<IActionResult> DeleteItem(string itemCode)
        {
            try
            {
                var result = await _itemsService.DeleteItemAsync(itemCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Mahsulotni o'chirishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{itemCode}")]
        public async Task<IActionResult> GetItemByIdAsync(string itemCode)
        {
            try
            {
                var result = await _itemsService.GetItemByIdAsync(itemCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Mahsulotni olishda xatolik yuz berdi!",
                    Error = ex.Message
                });
            }
        }
    }
}
