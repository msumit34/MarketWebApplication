using Newtonsoft.Json;

namespace TestApplication.Models.Barchart
{


    public class RatingsBarChart
    {
        public string id { get; set; }

        public string type { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public double AnalystRating { get; set; }
        public double HighTarget { get; set; }
        public double MeanTarget { get; set; }
        public double LowTarget { get; set; }
        public double CashFlowAnnual { get; set; }
        public double CashFlowPriorYear { get; set; }
        public double CashFlowTwoYearsPrior { get; set; }
        public double CurrAssetsQuarter { get; set; }
        public double CurrAssetsPriorQuarter { get; set; }
        public double CurrLiabilitiesPriorQuarter { get; set; }
        public string ShortTermSignal { get; set; }
        public string MedTermSignal { get; set; }
        public string LongTermSignal { get; set; }
        
        public string dateOfEntry { get; set; }

        public RatingsBarChart()
        {

        }

        public List<RatingsBarChart> setRatingsBarChartList(string[][] values)
        {
            try
            {
                List<RatingsBarChart> ratingsBarChart = new List<RatingsBarChart>();
                double result = 0.00;
                foreach (var v in values)
                {
                    if (v.Count() < 16)
                        continue;
                    ratingsBarChart.Add(new RatingsBarChart()
                    {
                        Symbol = v[0].ToString().Replace("\\", ""),
                        Name = v[1].ToString().Replace("\\", ""),
                        Industry = v[2].ToString().Replace("\\", ""),
                        AnalystRating = Double.TryParse(v[3].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        HighTarget = Double.TryParse(v[4].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        MeanTarget = Double.TryParse(v[5].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        LowTarget = Double.TryParse(v[6].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CashFlowAnnual = Double.TryParse(v[7].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CashFlowPriorYear = Double.TryParse(v[8].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CashFlowTwoYearsPrior = Double.TryParse(v[9].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrAssetsQuarter = Double.TryParse(v[10].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrAssetsPriorQuarter = Double.TryParse(v[11].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        CurrLiabilitiesPriorQuarter = Double.TryParse(v[12].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        ShortTermSignal = v[13].ToString().Replace("\\", ""),
                        MedTermSignal = v[14].ToString().Replace("\\", ""),
                        LongTermSignal = v[15].ToString().Replace("\\", ""), 
                        dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd")

                    });
                }

                return ratingsBarChart;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

    }
}
