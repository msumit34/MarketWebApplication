namespace TestApplication.Service
{
    public class EconomicsService
    {
        DatabaseAccessLayer database { get; set; }

        public EconomicsService()
        {
            this.database = new DatabaseAccessLayer(); 
        }

        public async Task<List<T>> getEconomicData<T>(string parameter1, string parameter2, string parameter3)
        {
            try
            {
                string sql = $"SELECT * FROM c WHERE c.Date >= '{parameter1}' AND c.Date <= '{parameter2}' AND c.type = '{typeof(T).Name}'";
                return await this.database.cosmosClient.GetItemsFromCosmosDB<T>(sql, "Economic");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
