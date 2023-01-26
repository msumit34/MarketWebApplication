using System.Net.Http;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.Common;
using Newtonsoft.Json;
using TestApplication.Models;
using Newtonsoft.Json.Linq;
using TestApplication.Models.ETL;
using System.Text;
using TestApplication.Database;
using TestApplication.Models.Barchart;

namespace TestApplication.Service.ETLService
{

    public class ETLService
    {
        public string iexBaseLink { get; set; }
        public string symbolsLink { get; set; }
        private HttpClient httpClient { get; set; }
        public List<CashFlowStatement> cashFlowStatement { get; set; }
        public List<BalanceSheet> balanceSheets { get; set; }
        public List<AdvancedStatistics> advancedStatistics { get; set; }
        public List<IncomeStatement> incomeStatement { get; set; }
        public List<Symbol> symbols { get; set; }
        public List<Company> companies { get; set; }
        public DatabaseAccessLayer database { get; set; }

        public DatabaseCustom databaseCustom { get; set; }

        public DatabaseRead databaseRead { get; set; }

        public DatabaseWrite databaseWrite { get; set; }
        
        public Dictionary<string, string> api_links { get; set; }

        public ETLService()
        
        {
            try
            {
                this.httpClient = new HttpClient();
                this.cashFlowStatement = new List<CashFlowStatement>();
                this.balanceSheets = new List<BalanceSheet>();
                this.advancedStatistics = new List<AdvancedStatistics>();
                this.incomeStatement = new List<IncomeStatement>();
                this.companies = new List<Company>();
                this.database = new DatabaseAccessLayer();
                this.databaseCustom = new TestApplication.Database.DatabaseCustom();
                this.databaseWrite = new TestApplication.Database.DatabaseWrite();
                api_links = new Dictionary<string, string>();
                JObject data = JObject.Parse(File.ReadAllText(@"C:\Users\malho\source\repos\TestApplication\TestApplication\API.json"));
                api_links.Add("base_link", data["api_base_link"].ToString());  
                api_links.Add("symbols", data["api_symbols_link"].ToString()); 
                api_links.Add("api_key", data["api_key"].ToString());  

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Symbol>> loadSymbols()
        {
            try
            {
                var count = await this.databaseCustom.customDatabaseQuery("SELECT COUNT(*) FROM Symbol;");
                if ( count.Count() == 0 || (count.Count() != 0 && count.Values.FirstOrDefault() == 0 ) )
                {
                   var response =  await this.httpClient.GetAsync(api_links["base_link"].ToString() + api_links["symbols"].ToString() + "?token=" + api_links["api_key"].ToString());
                   if (response.IsSuccessStatusCode)
                   {
                       var content = await response.Content.ReadAsStringAsync();
                       this.symbols = (List<Symbol>)JsonConvert.DeserializeObject<List<Symbol>>(content.ToString());
                   }
                }
                return this.symbols;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<Company>> loadCompanies(List<Company> companies)
        {
            try
            {
                foreach (var company in companies)
                {
                    await this.database.cosmosClient.CreateItemInCosmosDB<Company>(company, company.id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<bool> loadCashFlowStatement()
        {
            try
            {

                foreach(var symbol in symbols)
                {
                    var response = await this.httpClient.GetAsync(this.iexBaseLink + $"/stock/{symbol}/cash-flow");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(content.ToString());
                        CashFlowStatement cs = JsonConvert.DeserializeObject<CashFlowStatement>(obj["symbol"].ToString());
                        this.cashFlowStatement.Add(cs);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public async Task<bool> loadBalanceSheet()
        {
            try
            {
                foreach (var symbol in symbols)
                {
                    var response = await this.httpClient.GetAsync(this.iexBaseLink + $"/stock/{symbol}/balance-sheet");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(content.ToString());
                        BalanceSheet bs = JsonConvert.DeserializeObject<BalanceSheet>(obj["symbol"].ToString());
                        this.balanceSheets.Add(bs);
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public async Task<bool> loadIncomeStatement()
        {
            try
            {
                foreach(var symbol in symbols)
                {
                    var response = await this.httpClient.GetAsync(this.iexBaseLink + $"/stock/{symbol}/income");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(content.ToString());
                        IncomeStatement ist = JsonConvert.DeserializeObject<IncomeStatement>(obj["symbol"].ToString());
                        this.incomeStatement.Add(ist);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false; 
        }
        public async Task<bool> loadAdvancedStats()
        {
            try
            {
                foreach (var symbol in symbols)
                {
                    var response = await this.httpClient.GetAsync(this.iexBaseLink + $"/stock/{symbol}/advanced-stats");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(content.ToString());
                        AdvancedStatistics adv = JsonConvert.DeserializeObject<AdvancedStatistics>(obj["symbol"].ToString());
                        this.advancedStatistics.Add(adv);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<List<FinancialStatement>> getFinancialStatements()
        {
            try
            {
                var query = "SELECT * FROM c WHERE c.type='FinancialStatementDetails'";
                return await this.database.cosmosClient.GetItemsFromCosmosDB<FinancialStatement>(query, "Company");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<Company>> getCompanies()
        {
            try
            {
                var query = "SELECT * FROM c WHERE c.type='Company'";
                List<Company> companies = await this.database.cosmosClient.GetItemsFromCosmosDB<Company>(query);
                return companies;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<bool> loadFinancialRatiosBCData(List<FinancialRatiosBarChart> financialRatiosData )
        {
            try
            {
                if (financialRatiosData.Count == 0 || financialRatiosData == null)
                    return false;
                await this.databaseWrite.writeToCosmosDB<FinancialRatiosBarChart>(financialRatiosData);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> loadGrowthBCData(List<GrowthBarChart> growthData)
        {
            try
            {
                if (growthData.Count() == 0 || growthData == null)
                    return false;
                await this.databaseWrite.writeToCosmosDB<GrowthBarChart>(growthData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> loadRatingsBCData(List<RatingsBarChart> ratingsData)
        {
            try
            {
                if (ratingsData.Count == 0 || ratingsData == null)
                    return false;
                await this.databaseWrite.writeToCosmosDB<RatingsBarChart>(ratingsData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> loadTechnicalIndicatorBCData(List<TechnicalIndicatorsBarChart> technicalData)
        {
            try
            {
                if (technicalData.Count == 0 || technicalData == null)
                    return false;
                await this.databaseWrite.writeToCosmosDB<TechnicalIndicatorsBarChart>(technicalData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> loadEconomicData<Economic>(List<Economic> data)
        {
            try
            {
                if (data != null && data.Count() > 0)
                {
                    await this.databaseWrite.writeToCosmosDB<Economic>((List<Economic>)data, "Economic");
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> loadAllInstances()
        {
            try
            {
                await this.loadBalanceSheet();
                await this.loadAdvancedStats();
                await this.loadIncomeStatement();
                await this.loadCashFlowStatement();

                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false; 
        }
    }
}
