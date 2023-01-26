using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class CashFlowStatement
    {
        public CashFlowStatement()
        {

        }

        [JsonProperty("symbol")]
        public string symbol { get; set; }
        [JsonProperty("capitalExpenditures")]
        public double capitalExpenditures { get; set; }
        [JsonProperty("cashChange")]
        public double cashChange { get; set; }
        [JsonProperty("cashFlow")]
        public double cashFlow { get; set; }
        [JsonProperty("cashFlowFinancing")]
        public double cashFlowFinancing { get; set; }
        [JsonProperty("changesInInventories")]
        public double changesInInventories { get; set; }
        [JsonProperty("changesInReceivables")]
        public double changesInReceivables { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("depreciation")]
        public double depreciation { get; set; }
        [JsonProperty("dividendsPaid")]
        public double dividendsPaid { get; set; }
        [JsonProperty("exchangeRateEffect")]
        public string exchangeRateEffect { get; set; }
        [JsonProperty("filingType")]
        public string filingType { get; set; }
        [JsonProperty("fiscalDate")]
        public string fiscalDate { get; set; }
        [JsonProperty("fiscalQuarter")]
        public int fiscalQuarter { get; set; }
        [JsonProperty("fiscalYear")]
        public int fiscalYear { get; set; }
        [JsonProperty("investingActivityOther")]
        public string investingActivityOther { get; set; }
        [JsonProperty("investments")]
        public string investments { get; set; }
        [JsonProperty("netBorrowings")]
        public double netBorrowings { get; set; }
        [JsonProperty("netIncome")]
        public double netIncome { get; set; }
        [JsonProperty("otherFinancingCashFlows")]
        public double otherFinancingCashFlows { get; set; }
        [JsonProperty("reportDate")]
        public string reportDate { get; set; }
        [JsonProperty("totalInvestingCashFlows")]
        public double totalInvestingCashFlows { get; set; }

    }
}
