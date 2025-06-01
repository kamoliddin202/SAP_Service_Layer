namespace DTOs.ChartOfAccountDtos
{
    public class UpdateChartOfAccountDto
    {
        public string Name { get; set; }
        public int AccountLevel { get; set; }
        public string AccountType { get; set; }
        public string CashAccount { get; set; }
        public double Balance { get; set; }
        public string FatherAccountKey { get; set; }
    }
}
