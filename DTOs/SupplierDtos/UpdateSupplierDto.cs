namespace DTOs.SupplierDtos
{
    public class UpdateSupplierDto
    {
        public string CardName { get; set; }
        public string CardType { get; set; } = "S"; // Supplier
        public string Currency { get; set; }
        public string Phone1 { get; set; }
        public string EmailAddress { get; set; }
    }
}
