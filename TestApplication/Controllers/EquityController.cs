using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using TestApplication.Models;
using TestApplication.Models.Barchart;
using TestApplication.Service;
using Newtonsoft.Json;

namespace TestApplication.Controllers
{
    [Controller]
    [Route("Equity")]
    [Produces("application/json")]
    public class EquityController : Controller
    {
        private readonly ILogger<EquityController> _logger;
        private EquityService _service { get; set; }

        public EquityController(ILogger<EquityController> logger) : base()
        {
            _logger = logger;
            _service = new EquityService();
        }

        [HttpGet]
        [Route("EquityView")]
        public ActionResult Equity()
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

        [HttpGet("stockData")]
        public async Task<ActionResult<List<dynamic>>> stockData([FromQuery] string companyName, [FromQuery] DateTime DateOfEntry, [FromQuery] string financialDetails, [FromQuery] bool period, [FromQuery] string includeIndustry)
        {
            try
            {
                if (companyName.Length < 0)
                    return BadRequest("Bad Request, company names are null");

                List<dynamic> detailsList = new List<dynamic>();
                string[] companyNames = companyName.Split(",");
                foreach (var c in companyNames)
                {
                    if (companyName.Equals(""))
                    {
                        detailsList.Add(new StockDetails());
                        continue;
                    }
                    string date = DateOfEntry.ToString("yyyy_MM_dd");
                    if (includeIndustry.Equals("include"))
                    {
                        detailsList.Add(await this._service.getStockDetails(c, date,  financialDetails));
                        detailsList.Add(this._service.getIndustryDetails(true, detailsList.FirstOrDefault().technicals.Industry, detailsList.FirstOrDefault().technicals.Symbol));
                    }
                    else
                    {
                        detailsList.Add(await this._service.getStockDetails(c, date, financialDetails));
                    }
                }
                JsonConvert.SerializeObject(detailsList);
                return detailsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpPost("multiStockData")] 
        public async Task<ActionResult<List<StockDetails>>> getMultipleStockDetails([FromQuery] string[] company, [FromQuery] DateTime dateOfEntry, [FromQuery] string financialDetails, [FromQuery] string period)
        {
            try
            {
                List<StockDetails> details = new List<StockDetails>();
                string date = dateOfEntry.ToString("yyyy_MM_dd");
                if (company.Length == 0)
                    return BadRequest("No companies included in request.");
                foreach (var c in company)
                {
                    var result = await this._service.getStockDetails(c, date, financialDetails);
                    details.Add(result);
                }
                return Ok(details);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("generate/{value}")]
        public async Task<IActionResult> generate([FromRoute] string value)
        {
            try
            {
                return Content("Hello");
            }
            catch(Exception ex)
            {
                return Content("");
            }

            return NoContent();
        }
    }
}
