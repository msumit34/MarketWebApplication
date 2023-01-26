using TestApplication.Models;

namespace TestApplication.Service.Extensions
{
    public class StockValuation
    {

        public Stock stock { get; set; }
        public StockValuation()
        { 
        
        }

        public double stockValuation()
        {
            try
            {
                return this.getCurrentDividend() / ( this.expectedRateOrReturn() - this.getDividendGrowthRate());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double expectedRateOrReturn()
        {
            try
            {
                return (this.stock.dividend / this.stock.Last) + this.getDividendGrowthRate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double capitalGainsYield()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double pEValueation()
        {
            try
            {
                return (this.stock.earningsPerShare * (this.stock.Last / this.stock.earningsPerShare));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double getCurrentDividend()
        {
            try
            {
                return this.stock.dividend;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double getDividendGrowthRate()
        {
            try
            {

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

    }
}
