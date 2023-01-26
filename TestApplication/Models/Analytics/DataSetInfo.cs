using Newtonsoft.Json;


namespace TestApplication.Models.Analytics
{
    public class DataSetInfo
    {
        public DataSetInfo()
        {

        }

        [JsonProperty("NumberOfColumns")]
        public int numberOfColumns { get; set; }

        [JsonProperty("NumberOfRows")]
        public int numberOfRows { get; set; }

        [JsonProperty("Columns")]
        public string[] columns { get; set; }

        [JsonProperty("Normalized")]
        public bool normalized { get; set; }

        [JsonProperty("ColumnSummaryStatistics")]
        public List<string[]> summaryStatistics { get; set; }

        [JsonProperty("DataSet")]
        public List<string[]> dataSet { get; set; }
    }
}
