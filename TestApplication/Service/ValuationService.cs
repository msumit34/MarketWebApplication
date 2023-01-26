using TestApplication.Database;

namespace TestApplication.Service
{
    public class ValuationService
    {

        private DatabaseCustom _databaseCustom { get; set; }

        public ValuationService()
        {
            this._databaseCustom = new DatabaseCustom();
        }

        public async Task<double> ConstantGrowthStockValuation(double yearsHeld_n, string symbol)
        {
            try
            {
                double stockValue = 0;
                double initialDividend_d0 = 0;
                double growthRate_g = 0;
                var dividendHistory = await this._databaseCustom.customDatabaseQuery($"SELECT dividendYield FROM ADVANCED_DIVIDENDS WHERE symbol = {symbol};");
                var currentPrice = await this._databaseCustom.customDatabaseQuery($"SELECT Last FROM AdvancedStatistics WHERE symbol = {symbol};");
                var last = (double)currentPrice.Values.ToList()[0];
                List<dynamic> dividendValueList = dividendHistory.Values.ToList();
                dividendValueList = dividendValueList.Distinct().ToList();

                for (int i = 0; i < dividendValueList.Count - 1; i++)
                {
                    double difference = dividendValueList[i + 1] - dividendValueList[i];
                    growthRate_g += difference;
                }
                growthRate_g = growthRate_g / dividendValueList.Count();
                var rateOfReturn = this.expectedRateOfReturn(dividendValueList.Last(), last, growthRate_g);
                var expectedDividend = this.expectedDividend(growthRate_g, dividendValueList.Last());
                stockValue = (expectedDividend / ( rateOfReturn - growthRate_g ) );
                return stockValue;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }

            return 0.0;
        }

        public double expectedRateOfReturn(double lastDividend, double currentPrice, double growthRate)
        {
            try
            {
                return (lastDividend / currentPrice) + growthRate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return 0.0;
        }

        public double expectedDividend(double growthRate, double lastDividend)
        {
            try
            {
                return (lastDividend * (1 + growthRate));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return 0.0;
        }

        public double NonConstantGrowthStockValuation()
        {
            try
            {
                return 0.0;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public async Task<double> PERatioStockValuation(string symbol)
        {
            try
            {
                var peRatioList = await this._databaseCustom.customDatabaseQuery($"SELECT peRatio FROM AdvancedStatistics WHERE symbol = {symbol};");
                var EPS = await this._databaseCustom.customDatabaseQuery($"SELECT EPS FROM AdvancedStatistics WHERE symbol = {symbol};");
                double epsValue = EPS.Values.ToList()[0];
                List<dynamic> peList = peRatioList.Values.ToList();
                double peAverage = 0.0;
                for (int i = 0; i < peList.Count; i++)
                {
                    peAverage += peList[i];
                }
                peAverage = peAverage / peList.Count();
                return peAverage * epsValue;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return 0.0;
        }
    }
}
