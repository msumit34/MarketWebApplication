using Newtonsoft.Json;
using System.Security.Permissions;

namespace TestApplication.Models.Barchart
{
    public class GrowthBarChart
    {
        public string id { get; set; }

        public string type { get; set; }
        public string Symbol {get; set;}
         public string Name  {get; set;}  
         public double Last {get; set;}
         public double HighTarget {get; set;}
         public double MeanTarget {get; set;}
         public double FiftyTwoWeekHigh {get; set;}
         public double FiftyTwoWeekLow  {get; set;}
         public double NetIncomeGrowthAnnual {get; set;}
         public double SalesGrowthAnnual {get; set;}
         public double CashFlowGrowthAnnual {get; set;}
         public double CurrAssetsAnnual  {get; set;}
         public double CurrLiabilitiesAnnual {get; set;}
         public double DebtEquity {get; set;} 
         public double PriceCashFlow {get; set;}
         public double PriceSales {get; set;}
         public double PriceBook {get; set;}
         public double ROAGrowth {get; set;}
         public double ROEGrowth  {get; set;}

         public string Industry { get; set; }

         public string dateOfEntry { get; set; }

        public GrowthBarChart()
        { 
        
        }

        public List<GrowthBarChart> setListOfGrowthDataApi(string[][] value)
        {
            try
            {
                List<GrowthBarChart> growthBarChart = new List<GrowthBarChart>();
                double result = 0.00;
                foreach (var v in value)
                {
                    if (v.Count() < 19)
                        continue;
                    growthBarChart.Add(new GrowthBarChart()
                    {
                           Symbol = v[0].ToString().Replace("\\", ""),
                           Name = v[1].ToString().Replace("\\", ""),
                           Last = Double.TryParse(v[2].ToString().Replace("+", "").Replace("%", ""), out result)? result :0,
                           HighTarget = Double.TryParse(v[3].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           MeanTarget = Double.TryParse(v[4].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           FiftyTwoWeekHigh = Double.TryParse(v[5].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           FiftyTwoWeekLow = Double.TryParse(v[6].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           NetIncomeGrowthAnnual = Double.TryParse(v[7].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           SalesGrowthAnnual = Double.TryParse(v[8].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           CashFlowGrowthAnnual = Double.TryParse(v[9].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           CurrAssetsAnnual = Double.TryParse(v[10].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           CurrLiabilitiesAnnual = Double.TryParse(v[11].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           DebtEquity = Double.TryParse(v[12].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           PriceCashFlow = Double.TryParse(v[13].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           PriceSales = Double.TryParse(v[14].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           PriceBook = Double.TryParse(v[15].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           ROAGrowth = Double.TryParse(v[16].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                           ROEGrowth = Double.TryParse(v[17].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0, 
                           Industry = v[18].ToString(),
                           dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd")
                    });
                }

                return growthBarChart;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
