namespace DTOs.UpdateItemDtos
{
    public class GoodsReceiptCreateDto
    {
        public string ItemCode { get; set; }   // Mahsulot kodi
        public double Quantity { get; set; }   // Omborga qo‘shiladigan miqdor
        public string WarehouseCode { get; set; }  // Ombor kodi
    }
}
