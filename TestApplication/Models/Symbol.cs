using Newtonsoft.Json;

namespace TestApplication.Models
{
    public class Symbol
    {
        public Symbol()
        {

        }

        public Symbol(string symbol, string name, string date, 
            string region, string currency)
        {
            this.symbol = symbol;
            this.name = name;
            this.date = date;
            this.region = region;
            this.currency = currency;
        }


        [JsonProperty("symbol")]
        public string symbol { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("date")]
        public string date { get; set; }
        [JsonProperty("region")]
        public string region { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }


    }


    public class FinancialStatement
    {
        public FinancialStatement()
        { 
        
        }

        [JsonProperty("StatementName")]
        public string StatementName { get; set; }
    
    }
}
