using DataAccessLayer.Models;
using DTOs.UpdateItemDtos;

namespace BusinesssLogicLayer.Interfaces
{
    public interface IItemsService
    {
        Task<string> GetItemsAsync(string? itemCode, string? itemName, string? foreignName, int page, int pageSize);
        Task<string> PostItemAsync(Item body);
        Task<string> PatchItemAsync(string imtemCode, UpdateItemDto updateBody);
        Task<string> DeleteItemAsync(string ItemCode);
        Task<string> GetItemByIdAsync(string ItemCode);
    }
}


