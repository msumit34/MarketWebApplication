using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class BalanceSheet
    {
        public BalanceSheet()
        { 
        
        }

        public BalanceSheet(string symbol, string reportDate, 
            string filingType, string fiscalDate, int fiscalQuarter, 
            int fiscalYear, string currency, double currentCash, 
            double shortTermInvestments, double receivables, double inventory,
            double otherCurrentAssets, double currentAssets, double longTermInvestments, 
            double propertyPlantEquipment, double goodwill, double intagibleAssets, 
            double otherAssets, double totalAssets)
        { 
        
        }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("reportDate")]
        public string reportDate { get; set; }
        [JsonProperty("filingType")]
        public string filingType { get; set; }
        [JsonProperty("fiscalDate")]
        public string fiscalDate { get; set; }
        [JsonProperty("fiscalQuarter")]
        public int fiscalQuarter { get; set; }
        [JsonProperty("fiscalYear")]
        public int fiscalYear { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("currentCash")]
        public double currentCash { get; set; }
        [JsonProperty("shortTermInvestments")]
        public double shortTermInvestments { get; set; }
        [JsonProperty("receivables")]
        public double recievables { get; set; }
        [JsonProperty("inventory")]
        public double inventory { get; set; }
        [JsonProperty("otherCurrentAssets")]
        public double otherCurrentAssets { get; set; }
        [JsonProperty("currentAssets")]
        public double currentAssets { get; set; }
        [JsonProperty("longTermInvestments")]
        public double longTermInvestments { get; set; }
        [JsonProperty("propertyPlantEquipment")]
        public double propertyPlantEquipment { get; set; }
        [JsonProperty("goodwill")]
        public double goodwill { get; set; }
        [JsonProperty("intangibleAssets")]
        public double intangibleAssets { get; set; }
        [JsonProperty("otherAssets")]
        public double otherAssets { get; set; }
        [JsonProperty("totalAssets")]
        public double totalAssets { get; set; }
        
        [JsonProperty("accountsPayable")]
        public double accountsPayable { get; set; }
        [JsonProperty("currentLongTermDebt")]
        public double currentLongTermDebt { get; set; }
        [JsonProperty("otherCurrentLiabilities")]
        public double otherCurrentLiabilities { get; set; }
        [JsonProperty("totalCurrentLiabilities")]
        public double totalCurrentLiabilities { get; set; }
        [JsonProperty("longTermDebt")]
        public double longTermDebt { get; set; }
        [JsonProperty("otherLiabilities")]
        public double otherLiabilities { get; set; }
        [JsonProperty("minorityInterest")]
        public double minorityInterest { get; set; }
        [JsonProperty("totalLiabilities")]
        public double totalLiabilities { get; set; }
        [JsonProperty("commonStock")]
        public double commonStock { get; set; }
        [JsonProperty("retainedEarnings")]
        public double retainedEarnings { get; set; }
        [JsonProperty("treasuryStock")]
        public double treasuryStock { get; set; }
        [JsonProperty("capitalSurplus")]
        public double capitalSurplus { get; set; }
        [JsonProperty("shareholderEquity")]
        public double shareholderEquity { get; set; }
        [JsonProperty("netTangibleAssets")]
        public double netTangibleAssets { get; set; }
    }
}
