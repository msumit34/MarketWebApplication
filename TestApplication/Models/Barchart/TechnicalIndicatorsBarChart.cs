namespace TestApplication.Models
{
    public class TechnicalIndicatorsBarChart
    {

        public string id { get; set; }

        public string type { get; set; }
        public string Symbol { get; set; }
        public string FullName { get; set; }
        public string Industry { get; set; }
        public double OneMonthMovingAverage { get; set; }
        public double FiftyDayMovingAverage { get; set; }
        public double TwoHundredDayMovingAverage { get; set; }
        public double FiveDayRelationalStrength { get; set; }
        public double FiftyDayRelationalStrength { get; set; }
        public double OneHundredDayRelationalStrength { get; set; }
        public double PriceToCashFlow { get; set; }
        public double ReturnOnAssets { get; set; }
        public double PriceToEarningsGrowth { get; set; }
        public double EPSBasicLastQtr { get; set; }
        public double MeanTargetPrice { get; set; }
        public double HighTargetPrice { get; set; }
        public double TwentyDayPercent { get; set; }
        public double ThreeMonthPercent { get; set; }
        public double FiftyDayPercent { get; set; }
        public double OneHundredDayPercent { get; set; }
        public double ThreeMonthMovingAverage { get; set; }
        public double StandardDeviation { get; set; }
        public double SixtyMonthBeta { get; set; }
        public double FiftyTwoWeekChange { get; set; }

        public string dateOfEntry { get; set; }


        public TechnicalIndicatorsBarChart()
        {

        }

        public List<TechnicalIndicatorsBarChart> setListOfTechnicalIndicatorsBarChartApi(string[][] values)
        {
            try
            {
                List<TechnicalIndicatorsBarChart> stats = new List<TechnicalIndicatorsBarChart>();
                double result = 0.00;
                foreach (var v in values)
                {
                    if (v.Length != 24)
                        continue;
                    stats.Add(new TechnicalIndicatorsBarChart()
                    {
                        Symbol = v[0].ToString().Replace("\\", ""),
                        FullName = v[1].ToString().Replace("\\", "").Replace("'", ""),
                        Industry = v[2].ToString().Replace("\\", "").Replace("'", ""),
                        OneMonthMovingAverage = Double.TryParse(v[3].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiftyDayMovingAverage = Double.TryParse(v[4].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        TwoHundredDayMovingAverage = Double.TryParse(v[5].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiveDayRelationalStrength = Double.TryParse(v[6].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiftyDayRelationalStrength = Double.TryParse(v[7].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        OneHundredDayRelationalStrength = Double.TryParse(v[8].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        PriceToCashFlow = Double.TryParse(v[9].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        ReturnOnAssets = Double.TryParse(v[10].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        PriceToEarningsGrowth = Double.TryParse(v[11].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        EPSBasicLastQtr = Double.TryParse(v[12].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        MeanTargetPrice = Double.TryParse(v[13].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        HighTargetPrice = Double.TryParse(v[14].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        TwentyDayPercent = Double.TryParse(v[15].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        ThreeMonthPercent = Double.TryParse(v[16].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiftyDayPercent = Double.TryParse(v[17].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        OneHundredDayPercent = Double.TryParse(v[18].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        ThreeMonthMovingAverage = Double.TryParse(v[19].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        StandardDeviation = Double.TryParse(v[20].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        SixtyMonthBeta = Double.TryParse(v[21].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        FiftyTwoWeekChange = Double.TryParse(v[22].ToString().Replace("+", "").Replace("%", ""), out result) ? result : 0,
                        dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd")
                    }); 
                }

                return stats;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

    }
}
