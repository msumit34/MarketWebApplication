using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Data;
using TestApplication.Models.Analytics;
using TestApplication.Service;

namespace TestApplication.Controllers
{

    [Controller]
    [Route("Analytics")]
    [Produces("application/json")]
    public class AnalyticsController : Controller
    {

        private AnalyticsService service { get; set; }

        public AnalyticsController()
        {
            Analytics analytics = new Analytics();
            this.service = new AnalyticsService(); 
        }

        public ActionResult Analytics()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("LinearRegressionModel")]
        public async Task<ActionResult<RegressionMetrics>> getLinearRegressionModelResults([FromBody] string dependentVariable, [FromBody] string[] independentVariables, int[] partition)
        {
            try
            {
                if (dependentVariable.Equals("") || independentVariables.Count() == 0 || partition.Count() == 0)
                {
                    return BadRequest("Parameters were not create correctly");
                }

                return await this.service.GenerateLinearRegeressionModel(dependentVariable, independentVariables, partition);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("KMeansClusteringModel")]
        public async Task<ActionResult<KMeansClusteringModel>> getKMeansClusteringModelResults([FromBody] string[] features, [FromBody]int clusters)
        {
            try
            {
                if (clusters < 1 || features.Count() == 0)
                {
                    return BadRequest("Parameters are incorrect, ensure all parameters have been entered");
                }

                return await this.service.GenerateKMeansClusteringModel(features, clusters);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("DataSetInfo")]
        public async Task<ActionResult<DataSetInfo>> getDataSetInfo([FromQuery] string dataType)
        {
            try
            {
                 return await this.service.GenerateDataSetInfo(dataType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("DataSet")]
        public async Task<ActionResult<List<T>>> getDataSet<T>(T dataset)
        {
            try
            {
                if (dataset.Equals(""))
                    return BadRequest("Please select a dataset");
                return await this.service.GenerateDataSet<T>(dataset);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
    }
}
