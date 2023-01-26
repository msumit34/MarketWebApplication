namespace TestApplication.Service
{
    /**
     * Portfolio Service
     * 
     * Utilized to incorporate risk profile onto portfolio 
     * Understand the returns that are associated with a security and the probability of occurence
     *determine the risk profile of the portfolio including the beta, expected return , required return
     *and standard deviation
     *
     *What types of risk are involved in the portfolio
     * 
     * Systemic risks - Macro wide factors
     * Interest Rate, Inflation, Maturity, Liquidity, Exchange Rate, Political
     * 
     * Unsystemic risks - micro wide factors
     * Business, financial, default
     * 
     * Combined Risk - 
     * Total Risk, Corporate risk
     */


    public class PortfolioService
    {

        public double expectedReturn { get; set; }

        public double standardDeviation { get; set; }

        public double beta { get; set; }

        public double correlationOfVariance { get; set; }

        public Dictionary<string, double[]> return_prob_list { get; set; }

        public Dictionary<string, double[]> weight_beta_list { get; set; }

        public PortfolioService(Dictionary<string, double[]> return_prob_list, Dictionary<string, double[]> weight_beta_list)
        {
            this.expectedReturn = 0.0;
            this.standardDeviation = 0.0;
            this.beta = 0.0;
            this.correlationOfVariance = 0.0;
            this.return_prob_list = new Dictionary<string, double[]>();
            this.weight_beta_list = new Dictionary<string, double[]>();
        }

        public double expectedReturnOnPortfolio(Dictionary<string, double[]> return_prob_list)
        {
            try
            {
                //find a way to calculate the expected return and probability

                foreach(var x in return_prob_list.Values)
                {
                    this.expectedReturn += x[1] * x[0];  
                }

                return this.expectedReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double portfolioStandardDeviation()
        {
            try
            {
                double variance = 0.0;

                if (this.expectedReturn == 0.0)
                {
                    this.expectedReturn = this.expectedReturnOnPortfolio(this.return_prob_list);
                }

                foreach (var x in this.return_prob_list.Values)
                {
                    variance = Math.Pow((x[0] - this.expectedReturn), 2) * x[1];
                }

                this.standardDeviation = Math.Sqrt(variance);
                
                return this.standardDeviation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double portfolioRiskBeta()
        {
            try
            {
                foreach(var x in this.weight_beta_list.Values)
                {
                    this.beta += (x[0] * x[1]);
                }
                return this.beta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

      

        public double CorrelationOfVariance()
        {
            try
            {
                return this.standardDeviation / this.expectedReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        public double CapitalAssetPricingModel()
        {
            try
            {
                //required return on an asset- asset should be able to atleast return a risk free rate along with
                //a risk premium that reflects nondiversifiable risk on the stock
                //determines the minimum return required for it to be a sound investment

                //calculation is then
                //Required Return = Risk-free return + Premium for risk


                //where premium for risk is 
                //Risk Premium for Stock j = RPj = (Market Risk * Risk Free Rate) x Beta

                double riskFreeRate = 0.0;
                double marketRisk = 0.0;
                double beta = 0.0;

                return riskFreeRate + (marketRisk - riskFreeRate) * beta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }
    }
}
