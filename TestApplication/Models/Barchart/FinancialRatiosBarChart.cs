using Newtonsoft.Json;

namespace TestApplication.Models.Barchart
{
    public class FinancialRatiosBarChart
    {
        public string id { get; set; }

        public string type { get; set; }
        public string Symbol { get; set; }

        public string Industry { get; set; }
        public double CurrAssetsAnnual { get; set; }
        public double CurrAssetsPriorYear { get; set; }
        public double CurrLiabilitiesAnnual { get; set; }
        public double CurrLiabilitiesPriorYear { get; set; }
        public double CashFlowGrowthAnnual { get; set; }
        public double CashFlowGrowthPriorYear { get; set; }
        public double PriceToSales { get; set; }
        public double FiftyDayStochR { get; set; }
        public double HundredDayStochR { get; set; }
        public double FiveYearRevGrowth { get; set; }
        public double SalesAnnual { get; set; }
        public double SalesPriorYear { get; set; }
        public double SalesTwoYearsPrior { get; set; }
        public double NetIncomeGrowthAnnual { get; set; }
        public double NetIncomeGrowthPriorYear { get; set; }
        public double NetIncomeGrowthTwoYearsPrior { get; set; }
        public string HundredDayMACD { get; set; }
        public string FiftyDayMACD { get; set; }

        public string dateOfEntry { get; set; }

        public FinancialRatiosBarChart()
        {

        }

        public List<FinancialRatiosBarChart> setFinancialRatiosBarChartList(string[][] values)
        {
            try
            {
                List<FinancialRatiosBarChart> financialRatiosList = new List<FinancialRatiosBarChart>();
                double result = 0.00;
                foreach (var v in values)
                {
                    if (v.Count() < 20)
                        continue;
                    financialRatiosList.Add(new FinancialRatiosBarChart()
                    {
                        Symbol = v[0].ToString().Replace("\\", ""),
                        CurrAssetsAnnual = Double.TryParse(v[1].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrAssetsPriorYear = Double.TryParse(v[2].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrLiabilitiesAnnual = Double.TryParse(v[3].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrLiabilitiesPriorYear = Double.TryParse(v[4].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CashFlowGrowthAnnual = Double.TryParse(v[5].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CashFlowGrowthPriorYear = Double.TryParse(v[6].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        PriceToSales = Double.TryParse(v[7].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiftyDayStochR = Double.TryParse(v[8].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        HundredDayStochR = Double.TryParse(v[9].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiveYearRevGrowth = Double.TryParse(v[10].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        SalesAnnual = Double.TryParse(v[11].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        SalesPriorYear = Double.TryParse(v[12].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        SalesTwoYearsPrior = Double.TryParse(v[13].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        NetIncomeGrowthAnnual = Double.TryParse(v[14].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        NetIncomeGrowthPriorYear = Double.TryParse(v[15].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        NetIncomeGrowthTwoYearsPrior = Double.TryParse(v[16].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        HundredDayMACD = v[17].ToString(),
                        FiftyDayMACD = v[18].ToString(),
                        Industry = v[19].ToString(),
                        dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd")
                    }) ; 
                }

                return financialRatiosList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
