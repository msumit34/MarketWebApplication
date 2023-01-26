using Newtonsoft.Json;

namespace TestApplication.Models.Economics
{

    public class Economic
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("DateOfEntry")]
        public string dateOfEntry { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }

        public Economic() 
        {
        
        }

    }

    public class GDP : Economic
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }

        public GDP() : base()
        { 
        
        }

        public GDP( string date, string value, string type) : base()
        {
            this.Value = value;
            this.Date = date;
            this.dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd");
            this.type = type;
        }

        public List<GDP> setGDPList(string[][] values)
        {
            try
            {
                List<GDP> gdp = new List<GDP>();
                foreach (var val in values)
                {
                    if (val.Count() < 2)
                        continue;
                    gdp.Add(new GDP(val[0],val[1].Replace("\r", ""), "GDP"));
                }

                return gdp;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }

    public class InterestRate : Economic
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }

        public InterestRate():base()
        {

        }

        public InterestRate(string date, string value, string type) : base()
        {
            this.Value = value;
            this.Date = date;
            this.dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd");
            this.type = type;
        }

        public List<InterestRate> setInterestRateList(string[][] values)
        {
            try
            {
                List<InterestRate> ir = new List<InterestRate>();
                foreach (var val in values)
                {
                    if (val.Length < 2)
                        continue;
                    ir.Add(new InterestRate(val[0], val[1], "InterestRate"));
                }
                return ir;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }

    public class Unemployment : Economic
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        public Unemployment() : base()
        {

        }

        public Unemployment(string date, string value, string type) : base()
        {
            this.Value = value;
            this.Date = date;
            this.dateOfEntry = DateTime.Now.ToString("yyyy_MM_dd");
            this.type = type;
        }

        public List<Unemployment> setUnemploymentList(string[][] values)
        {
            try
            {
                List<Unemployment> unemployment = new List<Unemployment>();
                foreach (var val in values)
                {
                    if (val.Length < 2)
                        continue;
                    unemployment.Add(new Unemployment(val[0], val[1], "Unemployment"));
                }

                return unemployment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }

    public class GNP : Economic
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }

        public GNP() : base()
        {

        }

        public GNP(string date, string value, string type) : base()
        {
            this.Value = value;
            this.Date = date;
            this.type = type;
        }

        public List<GNP> setGNPList(string[][] values)
        {
            try
            {
                List<GNP> gnp = new List<GNP>();
                foreach (var val in values)
                {
                    if (val.Length < 2)
                        continue;
                    gnp.Add(new GNP(val[0], val[1], "GNP"));
                }

                return gnp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
