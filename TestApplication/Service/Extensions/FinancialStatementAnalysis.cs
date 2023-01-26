using TestApplication.Models;
using TestApplication.Models.Barchart;

namespace TestApplication.Service.Extensions
{
    public class FinancialStatementAnalysis
    {

        public Stock stock { get; set; }

        public FinancialStatementAnalysis(Stock stock )
        {
            this.stock = stock;
        }
  

        public double currentRatio()
        {
            try
            {
                return stock.currentAssets / stock.currentLiabilites;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double quickRatio()
        {
            try
            {
                return (this.stock.currentAssets - this.stock.inventory) / this.stock.currentLiabilites;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double inventoryTurnover()
        {
            try
            {
                return this.stock.costOfGoodsSold / this.stock.inventory;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double DaySalesOutstanding()
        {
            try
            {
                return this.stock.accountsReceivable / (this.stock.annualSales / 360);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double FixedAssetsTurnover()
        {
            try
            {
                return this.stock.annualSales / this.stock.netFixedAssets;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double TotalAssetsTurnover()
        {
            try
            {
                return this.stock.annualSales / this.stock.totalAssets;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double DebtRatio()
        {
            try
            {
                return this.stock.totalLiabilities / this.stock.totalAssets;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double TimesInterestEarned()
        {
            try
            {
                return this.stock.NetOperatingIncome / this.stock.interestCharges;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double FixedChargeCoverage()
        {
            try
            {
                return (this.stock.NetOperatingIncome + this.stock.leasePayment) /
                    this.stock.interestCharges + this.stock.leasePayment + (this.stock.sinkingFund / (1 - this.stock.taxRate));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double NetProfitMargin()
        {
            try
            {
                return this.stock.netIncome / this.stock.annualSales;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double ReturnOnAssets()
        {
            try
            {
                return this.stock.netIncome / this.stock.totalAssets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double ReturnOnEquity()
        {
            try
            {
                return this.stock.netIncome / this.stock.commonEquity;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double PriceEarnings()
        {
            try
            {
                return this.stock.marketPricePerShare / this.stock.earningsPerShare;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;     
        }

        public double MarketBook()
        {
            try
            {
                return this.stock.marketPricePerShare / this.stock.bookValuePerShare;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }
    }
}
