using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Text;
using System.Collections;
using System.Data.Sql;
using System.Data.SqlClient;

namespace TestApplication.Models
{
    public class Company 
    {

        [JsonProperty("symbol")]
        public string symbol { get; set; }
        [JsonProperty("companyName")]
        public string companyName { get; set; }
        [JsonProperty("exchange")]
        public string exchange { get; set; }
        [JsonProperty("industry")]
        public string industry { get; set; }
        [JsonProperty("sector")]
        public string sector { get; set; }
        
        public string type { get; set; }

        public string id { get; set; }

        public Company()
        { 
        
        }

        public Company(string symbol, string companyName, string exchange, 
            string industry, string sector)
        {
            this.symbol = symbol;
            this.companyName = companyName;
            this.exchange = exchange;
            this.industry = industry;
            this.sector = sector;
        }

        public List<Company> setListOfCompanyData(string[][] values)
        {
            try
            {
                List<Company> companies = new List<Company>();
                foreach (var value in values)
                {
                    if (value.Length < 2)
                        continue;
                    companies.Add(new Company()
                    {
                        symbol = value[0].Replace("\"", ""), 
                        companyName = value[1].Replace("\"", ""),
                        industry = value[2].Replace("\"",""),
                        exchange = value[3].Replace("\"", ""), 
                        sector = value[4].Replace("\"", ""),
                        type = "Company",
                        id = value[0] + "_" + DateTime.Now.ToString("MM_dd_yyyy")
                    });
                }
                return companies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
