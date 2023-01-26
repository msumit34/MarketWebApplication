using TestApplication.Database;
using TestApplication.Models;
using TestApplication.Models.Barchart;

namespace TestApplication.Service
{
    public class EquityService
    {
        public DatabaseRead database { get; set; }
        public List<AdvancedStatistics> advancedStats { get; set; }
        public List<BalanceSheet> balanceSheet { get; set; }

        public List<IncomeStatement> incomeStatement { get; set; }

        public List<CashFlowStatement> cashFlowStatement { get; set; }

        public EquityService()
        {
            this.database = new DatabaseRead();
            this.advancedStats = new List<AdvancedStatistics>();
            this.balanceSheet = new List<BalanceSheet>();
            this.incomeStatement = new List<IncomeStatement>();
            this.cashFlowStatement = new List<CashFlowStatement>();
        }

        public async Task<StockDetails> getStockDetails(string symbol, string date, string financialDetails)
        {
            try 
            {
                FinancialRatiosBarChart frbc = new FinancialRatiosBarChart();
                GrowthBarChart gbc = new GrowthBarChart();
                RatingsBarChart rbc = new RatingsBarChart();
                TechnicalIndicatorsBarChart tibc = new TechnicalIndicatorsBarChart();
                switch (financialDetails)
                {
                    case "Financial Ratios":
                        frbc = await this.getFinancialRatiosBarChart(symbol + "_" + date + "_FinancialRatiosBarChart");
                        break;
                    case "Growth":
                        gbc = await this.getGrowthBarChart(symbol + "_" + date + "_GrowthBarChart");
                        break;
                    case "Ratings":
                        rbc = await this.getRatingsBarChart(symbol + "_" + date + "_RatingsBarChart");
                        break;
                    case "TechnicalIndicators":
                        tibc = await this.getTechnicalIndicatorsBarChart(symbol + "_" + date + "_TechnicalIndicatorsBarChart");
                        break;
                    default:
                        frbc = await this.getFinancialRatiosBarChart(symbol + "_" + date + "_FinancialRatiosBarChart"); 
                        gbc = await this.getGrowthBarChart(symbol + "_" + date + "_GrowthBarChart");
                        rbc = await this.getRatingsBarChart(symbol + "_" + date + "_RatingsBarChart");
                        tibc = await this.getTechnicalIndicatorsBarChart(symbol + "_" + date + "_TechnicalIndicatorsBarChart");
                        break;
                }
                return new StockDetails(frbc, gbc, rbc, tibc);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public IndustryDetails getIndustryDetails(bool includeIndustry, string industry, string symbol)
        {
            try
            {
                return new IndustryDetails(includeIndustry, industry, symbol);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<BalanceSheet>> getBalanceSheet(string[]query)
        {
            try
            {
                this.database.OpenConnection();
                var res = await this.database.readDatabase<BalanceSheet>(query);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<CashFlowStatement>> getCashFlowStatement(string[] query)
        {
            try
            {
                this.database.OpenConnection();
                var res = await this.database.readDatabase<BalanceSheet>(query);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public async Task<List<IncomeStatement>> getIncomeStatement(string[] query)
        {
            try
            {
                this.database.OpenConnection();
                var res = await this.database.readDatabase<BalanceSheet>(query);

                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<AdvancedStatistics>> getAdvancedStatistics(string[] query)
        {
            try
            {
                this.database.OpenConnection();
                var res = await this.database.readDatabase<AdvancedStatistics>(query);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<FinancialRatiosBarChart> getFinancialRatiosBarChart(string id)
        {
            try
            {
                var result = await this.database.ReadFromCosmosDBAsync<FinancialRatiosBarChart>(id);
                return result;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public async Task<GrowthBarChart> getGrowthBarChart(string id)
        {
            try
            {
                var result = await this.database.ReadFromCosmosDBAsync<GrowthBarChart>(id);
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public async Task<RatingsBarChart> getRatingsBarChart(string id)
        {
            try
            {
                var result = await this.database.ReadFromCosmosDBAsync<RatingsBarChart>(id);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public async Task<TechnicalIndicatorsBarChart> getTechnicalIndicatorsBarChart(string id)
        {
            try
            {

                var result = await this.database.ReadFromCosmosDBAsync<TechnicalIndicatorsBarChart>(id);
                return result; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
}
