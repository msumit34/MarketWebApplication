using Microsoft.ML.Data;
using System.Data;
using System.Reflection;
using TestApplication.Models;
using TestApplication.Models.Analytics;
using TestApplication.Models.Barchart;

namespace TestApplication.Service
{
    public class AnalyticsService
    {
        private Analytics analytics { get; set; }

        public AnalyticsService()
        {
            this.analytics = new Analytics();
        }

        public async Task<RegressionMetrics> GenerateLinearRegeressionModel(string dependentVariable, string[] independentVariables, int[] partition)
        {
            try
            {
                return this.analytics.RunLinearRegression(dependentVariable, independentVariables, partition); 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<KMeansClusteringModel> GenerateKMeansClusteringModel(string[] features, int clusters)
        {
            try
            {
                return this.analytics.RunKMeansClusteringModel(features, clusters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<dynamic> GenerateDataSetInfo(string dataType)
        {
            try
            {
                dynamic dataSetInfo = null;
                switch (dataType)
                {
                    case "Financial Ratios":
                        await this.analytics.CreateDataViewForMLContext<FinancialRatiosBarChart>();
                        dataSetInfo = this.analytics.PrepareDataSetForMLContext(typeof(FinancialRatiosBarChart).GetProperties())._data;
                        break;
                    case "Growth":
                        await this.analytics.CreateDataViewForMLContext<GrowthBarChart>();
                        dataSetInfo = this.analytics.PrepareDataSetForMLContext(typeof(GrowthBarChart).GetProperties())._data;
                        break;
                    case "Ratings":
                        await this.analytics.CreateDataViewForMLContext<RatingsBarChart>();
                        dataSetInfo = this.analytics.PrepareDataSetForMLContext(typeof(RatingsBarChart).GetProperties())._data;
                        break;
                    case "Technical Indicators":
                        await this.analytics.CreateDataViewForMLContext<TechnicalIndicatorsBarChart>();
                        dataSetInfo = this.analytics.PrepareDataSetForMLContext(typeof(TechnicalIndicatorsBarChart).GetProperties())._data;
                        break;
                    default:
                        break;
                }
                return dataSetInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<T>> GenerateDataSet<T>(T dataset)
        {
            try
            {
                //return this.analytics.PrepareDataSetForMLContext(typeof())
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
