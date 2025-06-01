namespace DTOs.CutomerDtos
{
    public class UpdateCustomerDto
    {
        public string CardName { get; set; }
        public string Currency { get; set; }
        public string Phone1 { get; set; }
        public string EmailAddress { get; set; }
        public string CardType { get; set; } = "C";
    }
}
