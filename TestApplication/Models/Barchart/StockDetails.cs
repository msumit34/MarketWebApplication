using Newtonsoft.Json;
using System.Text;
using TestApplication.Database;

namespace TestApplication.Models.Barchart
{
    public class MarketDetails
    {

        internal DatabaseAccessLayer _database { get; set; }
        public MarketDetails()
        { 
        
        }
    
    }


    public class StockDetails : MarketDetails
    {
        [JsonProperty("Symbol")]
        public string symbol { get; set; }
        [JsonProperty("Name")]
        public string name { get; set; }
        [JsonProperty("Industry")]
        public string industry { get; set; }
        [JsonProperty("ratios")]
        public FinancialRatiosBarChart ratios { get; set; }
        [JsonProperty("growth")]
        public GrowthBarChart growth { get; set; }
        [JsonProperty("ratings")]
        public RatingsBarChart ratings { get; set; }
        [JsonProperty("technicals")]
        public TechnicalIndicatorsBarChart technicals { get; set; }
        [JsonProperty("salesGrowthAnnual")]
        public double? salesGrowthAnnual { get; set; }
        [JsonProperty("netIncomeGrowthAnnual")]
        public double? netIncomeGrowthAnnual { get; set; }
        [JsonProperty("cashFlowGrowth")]
        public double? cashFlowGrowth { get; set; }
        [JsonProperty("currentRatio")]
        public double? currentRatio { get; set; }
        [JsonProperty("shortTermSignal")]
        public string shortTermSignal { get; set; }
        [JsonProperty("medTermSignal")]
        public string medTermSignal { get; set; }
        [JsonProperty("OneMonthMovingAverage")]
        public double OneMonthMovingAverage { get; set; }
        [JsonProperty("FiftyDayMovingAverage")]
        public double FiftyDayMovingAverage { get; set; }
        [JsonProperty("TwoHundredDayMovingAverage")]
        public double TwoHundredDayMovingAverage { get; set; }
        [JsonProperty("FiveDayRelationalStrength")]
        public double FiveDayRelationalStrength { get; set; }
        [JsonProperty("FiftyDayRelationalStrength")]
        public double FiftyDayRelationalStrength { get; set; }
        [JsonProperty("OneHundredDayRelationalStrength")]
        public double OneHundredDayRelationalStrength { get; set; }
        [JsonProperty("DebtToEquity")]
        public double debtToEquity { get; set; }


        public StockDetails() : base()
        {
            this._database = new DatabaseAccessLayer();
        }

        public StockDetails(FinancialRatiosBarChart ratios, GrowthBarChart growth, RatingsBarChart ratings, 
            TechnicalIndicatorsBarChart technicals) : base()
        {

            if (ratios != null)
            {
                this.ratios = ratios;
                this.currentRatio = ratios.CurrAssetsAnnual / ratios.CurrLiabilitiesAnnual;
                if (this.currentRatio == double.NaN)
                {
                    this.currentRatio = 0;
                }
            }
            if (growth != null)
            {
                this.growth = growth;
                this.salesGrowthAnnual = (growth.SalesGrowthAnnual == null) ? 0 : growth.SalesGrowthAnnual;
                this.netIncomeGrowthAnnual = (growth.NetIncomeGrowthAnnual == null) ? 0 : growth.NetIncomeGrowthAnnual;
                this.cashFlowGrowth = (growth.CashFlowGrowthAnnual == null) ? 0 : growth.CashFlowGrowthAnnual;
                this.debtToEquity = this.growth.DebtEquity;
            }
            if (ratings != null)
            {
                this.ratings = ratings;
                this.shortTermSignal = ratings.ShortTermSignal;
                this.medTermSignal = ratings.MedTermSignal;
            }
            if (technicals != null)
            {
                this.technicals = technicals;
                this.OneMonthMovingAverage = technicals.OneMonthMovingAverage;
                this.FiftyDayMovingAverage = technicals.FiftyDayMovingAverage;
                this.TwoHundredDayMovingAverage = technicals.TwoHundredDayMovingAverage;
                this.FiveDayRelationalStrength = technicals.FiveDayRelationalStrength;
                this.FiftyDayRelationalStrength = technicals.FiftyDayRelationalStrength;
                this.OneHundredDayRelationalStrength = technicals.OneHundredDayRelationalStrength;
            }
            this._database = new DatabaseAccessLayer();

        }
    }

    public class IndustryDetails : MarketDetails
    {
        [JsonProperty("salesGrowthAnnualIndustryAverage")]
        public double salesGrowthAnnualIndustryAverage { get; set; }
        [JsonProperty("netIncomeGrowthAnnualIndustryAverage")]
        public double netIncomeGrowthAnnualIndustryAverage { get; set; }
        [JsonProperty("cashFlowGrowthIndustryAverage")]
        public double cashFlowGrowthIndustryAverage { get; set; }
        [JsonProperty("currentRatioIndustryAverage")]
        public double currentRatioIndustryAverage { get; set; }
        [JsonProperty("quickRatioIndustryAverage")]
        public double quickRatioIndustryAverage { get; set; }
        [JsonProperty("fiftyDayRelationalStrengthIndustryAverage")]
        public double fiftyDayRelationalStrengthIndustryAverage { get; set; }
        [JsonProperty("twoHundredDayMovingAverageDifferenceIndustryAverage")]
        public double twoHundredDayMovingAverageDifferenceIndustryAverage { get; set; }

        public IndustryDetails() : base()
        { 
        
        }

        public IndustryDetails(bool isIndustry, string industry, string symbol) : base()
        {
            try
            {
                if (isIndustry)
                {
                    this._database = new DatabaseAccessLayer();
                    this.setCurrentRatioIndustryAverage(industry, symbol);
                    this.setFiftyDayRelationalStrengthIndustryAverage(industry, symbol);
                    this.setNetIncomeGrowthAnnualIndustryAverage(industry, symbol);
                    this.setQuickRatioIndustryAverage(industry, symbol);
                    this.setSalesGrowthAnnualIndustryAverage(industry, symbol);
                    this.setTwoHundredDayMovingAverageIndustryAverage(industry, symbol);
                    this.setCashFlowGrowthIndustryAverage(industry, symbol);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void setSalesGrowthAnnualIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry + "\r"}' AND c.Symbol != '{symbol}'" };
                var resultSGAI = this._database.cosmosClient.GetItemsFromCosmosDB<GrowthBarChart>(query[0]).Result;
                this.salesGrowthAnnualIndustryAverage = resultSGAI.Select(x => x.SalesGrowthAnnual).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void setNetIncomeGrowthAnnualIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry + "\r"}' AND c.Symbol != '{symbol}'" };
                var resultNIGAI = this._database.cosmosClient.GetItemsFromCosmosDB<GrowthBarChart>(query[0]).Result;
                this.netIncomeGrowthAnnualIndustryAverage = resultNIGAI.Select(x => x.NetIncomeGrowthAnnual).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setCashFlowGrowthIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry + "\r"}' AND c.Symbol != '{symbol}'" };
                var result = this._database.cosmosClient.GetItemsFromCosmosDB<GrowthBarChart>(query[0]).Result;
                this.cashFlowGrowthIndustryAverage = result.Select(x => x.CashFlowGrowthAnnual).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setCurrentRatioIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry + "\r"}' AND c.Symbol != '{symbol}'" };
                var resultCR = this._database.cosmosClient.GetItemsFromCosmosDB<FinancialRatiosBarChart>(query[0]).Result;
                this.currentRatioIndustryAverage = resultCR.Select(x => (x.CurrAssetsAnnual / x.CurrLiabilitiesAnnual)).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void setQuickRatioIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT *  FROM c WHERE c.Industry = '{industry + "\r"}' AND c.Symbol != '{symbol}'" };
                var resultQR = this._database.cosmosClient.GetItemsFromCosmosDB<FinancialRatiosBarChart>(query[0]).Result;
                this.quickRatioIndustryAverage = resultQR.Select(x => (x.CurrAssetsAnnual - 0) / x.CurrLiabilitiesAnnual).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setFiftyDayRelationalStrengthIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry}' AND c.Symbol != '{symbol}'" };
                var resultFDRS = this._database.cosmosClient.GetItemsFromCosmosDB<TechnicalIndicatorsBarChart>(query[0]).Result;
                this.fiftyDayRelationalStrengthIndustryAverage = resultFDRS.Select(x => x.FiftyDayRelationalStrength).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setTwoHundredDayMovingAverageIndustryAverage(string industry, string symbol)
        {
            try
            {
                string[] query = new string[] { $"SELECT * FROM c WHERE c.Industry = '{industry}' AND c.Symbol != '{symbol}'" };
                var resultTHDMA = this._database.cosmosClient.GetItemsFromCosmosDB<TechnicalIndicatorsBarChart>(query[0]).Result;
                this.twoHundredDayMovingAverageDifferenceIndustryAverage = resultTHDMA.Select(x => x.TwoHundredDayMovingAverage).Average();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
