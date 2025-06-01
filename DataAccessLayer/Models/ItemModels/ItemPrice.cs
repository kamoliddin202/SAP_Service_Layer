namespace DataAccessLayer.Models.ItemModels
{
    public class ItemPrice
    {
        public int PriceList { get; set; }  // qo‘shish kerak
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
