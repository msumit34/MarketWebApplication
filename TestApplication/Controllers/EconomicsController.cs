using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TestApplication.Models.Economics;
using TestApplication.Service;

namespace TestApplication.Controllers
{

    [Controller]
    [Route("Economics")]
    [Produces("application/json")]
    public class EconomicsController : Controller
    {
        private readonly ILogger<EconomicsController> _logger;
        private EconomicsService service { get; set; }

        public EconomicsController(ILogger<EconomicsController> _logger) : base()
        {
            _logger = _logger;
            this.service = new EconomicsService(); 
        }

        [HttpGet]
        [Route("EconomicsView")]
        public ActionResult Economics()
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

        [HttpGet("GDPData")]
        public async Task<ActionResult<List<GDP>>> getGDPData([FromQuery] string dataType, [FromQuery] string startYear, [FromQuery] string endYear, [FromQuery] string country)
        {
            try
            {
                dynamic result = null;
                if ((!dataType.Equals("")) && Int16.TryParse(startYear, out short start) && Int16.TryParse(endYear, out short end) && (country.Equals("") || country != null))
                {
                    result = await this.service.getEconomicData<GDP>("01/01/"+ startYear, "12/31/" + endYear, country);
                    return result;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("GNPData")]
        public async Task<ActionResult<List<GNP>>> getGNPData([FromQuery] string dataType, [FromQuery] string startYear, [FromQuery] string endYear, [FromQuery] string country)
        {
            try
            {
                dynamic result = null;
                if ((!dataType.Equals("")) && Int16.TryParse(startYear, out short start) && Int16.TryParse(endYear, out short end) && (country.Equals("") || country != null))
                {
                    result = await this.service.getEconomicData<GNP>(startYear, endYear, country);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("UnemploymentData")]
        public async Task<ActionResult<List<Unemployment>>> getUnemploymentData([FromQuery] string dataType, [FromQuery] string startYear, [FromQuery] string endYear, [FromQuery] string country)
        {
            try
            {
                dynamic result = null;
                if ((!dataType.Equals("")) && Int16.TryParse(startYear, out short start) && Int16.TryParse(endYear, out short end) && (country.Equals("") || country != null))
                {
                    result = await this.service.getEconomicData<Unemployment>(startYear, endYear, country);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("InterestRateData")]
        public async Task<ActionResult<List<InterestRate>>> getInterestRateData([FromQuery] string dataType, [FromQuery] string startYear, [FromQuery] string endYear, [FromQuery] string country)
        {
            try
            {
                dynamic result = null;
                if ((!dataType.Equals("")) && Int16.TryParse(startYear, out short start) && Int16.TryParse(endYear, out short end) && (country.Equals("") || country != null))
                {
                    result = await this.service.getEconomicData<InterestRate>(startYear, endYear, country);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
    }
}
