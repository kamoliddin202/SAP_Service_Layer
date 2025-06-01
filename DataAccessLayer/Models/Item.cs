using DataAccessLayer.Models.ItemModels;

public class Item
{
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public string SalesItem { get; set; }
    public string InventoryItem { get; set; }
    public int ItemsGroupCode { get; set; }
    public string PurchaseItem { get; set; }
    public List<ItemPrice> ItemPrices { get; set; }
    public List<ItemWarehouseInfo> ItemWarehouseInfoCollection { get; set; }
}


