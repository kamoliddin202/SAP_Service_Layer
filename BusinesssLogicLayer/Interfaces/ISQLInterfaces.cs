namespace BusinesssLogicLayer.Interfaces
{
    public interface ISQLInterfaces
    {

        Task<string> ExecuteAllSqlAsync(string sqlText);
        Task<string> UmumiyNarx500danKattaSotuvlarAsync(); 
        Task<string> UmumiyNarx500danKattaMijozIsimlariBilanAsync();
        Task<string> BuOydagiBarchaSotuvlarAsyncAsync();
        Task<string> ShuOydaKelganPulniChiqarishAsync(DateTime fromDate, DateTime toDate);
        Task<string> ShuOydaSotuvlarsoniniChiqarishAsync();
        Task<string> GetSalesWithItemsAsync();
        Task<string> GetMostExpensiveSoldItemAsync();
        Task<string> GetTop10MostExpensiveItemsAsync();
        Task<string> GetMostExpensiveItemWithCustomerAsync();
        Task<string> GetPurchasesWithLinesAsync();



    }
}







//Task<string> EngQimmatSotilganMolniChiqarishAsync();
//Task<string> EngKopSotibOlgan10taMollarniChiqarishAsync();
//Task<string> EngQimmatSotiganMolMijozIsmiBilanAsync();
//Task<string> EngKopSotibOlganMijozlar10taAsync();
//Task<string> EngKopPulSarflaganMijozlar10taAsync();
//Task<string> BarchaHaridlarVaHaridlarQatorlariBilanAsync();


//Task<string> UmumiyNarx500danKattaSotuvlarAsync();
//Task<string> UmumiyNarx500danKattaMijozIsimlariBilanAsync();
//Task<string> BuOydagiBarchaSotuvlarAsyncAsync();
//Task<string> ShuOydaKelganPulniChiqarishAsync(DateTime fromDate, DateTime toDate);
//Task<string> ShuOydaSotuvlarsoniniChiqarishAsync();
////Task<string> HarbittaSotuvniSotilganMollariBilanAsync();

//Task<string> ExecuteSqlAsync(string sqlText);
//Task<string> GetSalesOver500Async();
//Task<string> GetSalesOver500WithCustomerAsync();
//Task<string> GetSalesThisMonthAsync();
//Task<string> GetTotalReceivedThisMonthAsync();
//Task<string> GetSalesCountThisMonthAsync();
//Task<string> GetSalesWithItemsAsync();
//Task<string> GetMostExpensiveSoldItemAsync();
//Task<string> GetTop10MostExpensiveItemsAsync();
//Task<string> GetMostExpensiveItemWithCustomerAsync();
//Task<string> GetTop10FrequentCustomersAsync();
//Task<string> GetTop10HighestSpendingCustomersAsync();
//Task<string> GetPurchasesWithLinesAsync();