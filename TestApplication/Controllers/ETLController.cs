using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TestApplication.Database;
using TestApplication.Models;
using TestApplication.Models.Barchart;
using TestApplication.Models.ETL;
using TestApplication.Service.ETLService;
using System.Reflection;
using TestApplication.Models.Economics;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("ETL")]
    [Produces("application/json")]
    public class ETLController : Controller
    {
        
        public ETLService service { get; set; }

        public TechnicalIndicatorsBarChart stats { get; set; }
        public GrowthBarChart growth { get; set; }
        public RatingsBarChart ratings { get; set; }
        public FinancialRatiosBarChart financial { get; set; }
        
        public Company company { get; set; }


        public ETLController() : base()
        {
            this.service = new ETLService();
            this.stats = new TechnicalIndicatorsBarChart();
            this.growth = new GrowthBarChart();
            this.ratings = new RatingsBarChart();
            this.financial = new FinancialRatiosBarChart();
            this.company = new Company(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            try
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            return View();
        }

        [HttpGet]
        [Route("GetSymbols")]
        public async Task<ActionResult<bool>> GetSymbols()
        {
            try
            {
                await this.service.loadSymbols();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("ETLView")]
        public async Task<ActionResult<string>> ETL()
        {
            return View();
        }


        [HttpGet]
        [Route("UpdateIncomeStatement")]
        public async Task<ActionResult<string>> UpdateIncomeStatement()
        {
            try
            {
                return Ok(await this.service.loadIncomeStatement());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("UpdateBalanceSheet")]
        public async Task<ActionResult<string>> UpdateBalanceSheet()
        {
            try
            {
                return Ok(await this.service.loadBalanceSheet());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("UpdateAdvancedStats")]
        public async Task<ActionResult<string>> UpdateAdvancedStats()
        {
            try
            {
                return Ok(await this.service.loadAdvancedStats());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("AllEquityData")]
        public async Task<ActionResult<string>> AllEquityData()
        {
            try
            {
                return Ok(await this.service.loadAllInstances());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("getVariableList")]
        public async Task<ActionResult<List<string>>> getVariableList([FromQuery] string dataType)
        {
            try
            {
                if (!dataType.Equals(""))
                {
                    List<string> prop = new List<string>();
                    PropertyInfo[] propertyInfo = null;
                    switch(dataType)
                    {
                        case "FinancialRatios":
                            propertyInfo = typeof(FinancialRatiosBarChart).GetProperties();
                            prop = propertyInfo.Select(x => x.Name).ToList<string>();
                            break;
                        case "Growth":
                            propertyInfo = typeof(GrowthBarChart).GetProperties();
                            prop = propertyInfo.Select(x => x.Name).ToList<string>();
                            break;
                        case "Ratings":
                            propertyInfo = typeof(RatingsBarChart).GetProperties();
                            prop = propertyInfo.Select(x => x.Name).ToList<string>();
                            break;
                        case "TechnicalIndicators":
                            propertyInfo = typeof(TechnicalIndicatorsBarChart).GetProperties();
                            prop = propertyInfo.Select(x => x.Name).ToList<string>();
                            break;
                    }
                    return prop;
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }
       
        


        [HttpGet]
        [Route("getFinancialStatements")]
        public async Task<ActionResult<List<FinancialStatement>>> getFinancialStatements()
        {
            try
            {
                return await this.service.getFinancialStatements();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("getCompanies")]
        public async Task<ActionResult<List<Company>>> getCompanies()
        {
            try
            {
                var result = await this.service.getCompanies();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("loadSymbols")]
        public async Task<ActionResult<bool>> loadSymbols([FromBody] string[][] values)
        {
            try
            {
                await this.service.loadCompanies(company.setListOfCompanyData(values));
                return true;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("loadTechnicalData")]
        public async Task<ActionResult<bool>> loadTechnicalData([FromBody] string[][] values)
        {
            try
            {
                if(values.Count() > 0)
                    await this.service.loadTechnicalIndicatorBCData(this.stats.setListOfTechnicalIndicatorsBarChartApi(values));
                
                return true; 
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("loadGrowthData")]
        public async Task<ActionResult<bool>> loadGrowthData([FromBody] string[][] growthData)
        {
            try
            {
                if (growthData.Length > 0)
                    await this.service.loadGrowthBCData(this.growth.setListOfGrowthDataApi(growthData));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("loadFinancialRatiosData")]
        public async Task<ActionResult<bool>> loadFinancialRatiosData([FromBody] string[][] financialRatioData)
        {
            try
            {
                if (financialRatioData.Length > 0)
                    await this.service.loadFinancialRatiosBCData(this.financial.setFinancialRatiosBarChartList(financialRatioData));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("loadRatingsData")]
        public async Task<ActionResult<bool>> loadRatingsData([FromBody] string[][] ratingsData)
        {
            try
            {
                if (ratingsData.Length > 0)
                    await this.service.loadRatingsBCData(this.ratings.setRatingsBarChartList(ratingsData));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("EconomicData")]
        public async Task<ActionResult<bool>> loadEconomicData([FromBody] string[][] economicData, [FromQuery]string dataType)
        {
            try
            { 
                switch(dataType)
                {
                    case "GDP":
                        GDP obj = new GDP();
                        await this.service.loadEconomicData<GDP>((List<GDP>)typeof(GDP).GetMethod($"set{dataType.Replace(" ", "")}List").Invoke(obj, new object[] { economicData }));
                        break;
                    case "GNP":
                        GNP gnp = new GNP();
                        await this.service.loadEconomicData<GNP>((List<GNP>)typeof(GNP).GetMethod($"set{dataType.Replace(" ", "")}List").Invoke(gnp, new object[] { economicData }));
                        break;
                    case "Interest Rate":
                        InterestRate rate = new InterestRate(); 
                        await this.service.loadEconomicData<InterestRate>((List<InterestRate>)typeof(InterestRate).GetMethod($"set{dataType.Replace(" ", "")}List").Invoke(rate, new object[] { economicData }));
                        break;
                    case "Unemployment":
                        Unemployment emp = new Unemployment();
                        await this.service.loadEconomicData<Unemployment>((List<Unemployment>)typeof(Unemployment).GetMethod($"set{dataType.Replace(" ", "")}List").Invoke(emp, new object[] { economicData }));
                        break;
                    default:
                        break;
                }

                return true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest();
        }
    }
}
