namespace TestApplication.Models.ETL
{
    public class ETLModel
    {
        public string[] symbols {get; set;}

        public string[] industry { get; set; }

        public string[] sector { get; set; }

        public string[] exchange { get; set; }

        public string[] etl { get; set; }

        public ETLModel()
        { 
        
        }

        public ETLModel(string[] symbols, string[] industry, string[] sector, 
                        string[] exchange, string[] etl)
        {
            this.symbols = symbols;
            this.industry = industry;
            this.sector = sector;
            this.exchange = exchange;
            this.etl = etl;
        }
    }
}
