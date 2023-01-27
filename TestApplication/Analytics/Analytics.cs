using Microsoft.Spark;
using Microsoft.Spark.Sql;
using Microsoft.Spark.ML;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Spark.Sql.Types;
using TestApplication.Models.Barchart;
using TestApplication.Models.Analytics;
using System.Reflection;
using Microsoft.ML.Trainers;
using System.Diagnostics.Eventing.Reader;
using System.Data;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.ML.Transforms;
using FastMember;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using System.Collections;
using Microsoft.Identity.Client;
using TestApplication.Models;
using System.Linq;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using System.Numerics;

namespace TestApplication
{
    public class Analytics
    {
        private SparkSession session { get; set; }

        private DataFrame frame { get; set; }

        private IReadOnlyList<string> columns { get; set; }

        private List<DataFrame> frames { get; set; }

        public IDataView dataView { get; set; }

        private MLContext mlContext { get; set; }

        private DatabaseAccessLayer database { get; set; }


        public Analytics()
        {
            this.database = new DatabaseAccessLayer();
            this.dataView = null;
            this.mlContext = new MLContext();
        }
        public bool StartSparkSession()
        {
            try
            {
                SparkContext sparkContext = new SparkContext();
                SparkConf conf = new SparkConf();
                conf.SetAppName("Market Analytics Pipeline").SetMaster("master").Set("driver-memory", "5000m").Set("executor-memory", "5000m");
                this.session = SparkSession.Builder().AppName("Market Analytics Pipeline").GetOrCreate();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> CreateDataFrame<T>()
        {
            try
            {
                var result = await this.database.cosmosClient.GetItemsFromCosmosDB<T>("SELECT * FROM c");
                PropertyInfo[] properties = result[0].GetType().GetProperties();
                List<StructField> fields = new List<StructField>();
                foreach (var prop in properties)
                {
                    if (!prop.PropertyType.FullName.Equals("System.String"))
                    {
                        fields.Add(new StructField(prop.Name, new DoubleType()));
                    }
                }
                StructType structType = new StructType(fields);
                List<GenericRow> genericRows = new List<GenericRow>();
                foreach (var r in result)
                {
                    object[] gRow = new object[fields.Count()];
                    for (int i = 0; i < fields.Count(); i++)
                    {
                        gRow[i] = r.GetType().GetProperty(fields[i].Name).GetValue(r);
                    }
                    genericRows.Add(new GenericRow(gRow));
                }
                this.frames.Add(this.session.CreateDataFrame(genericRows, structType));
                MergeFrames();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool MergeFrames()
        {
            try
            {
                foreach (DataFrame frame in this.frames)
                {
                    if (this.frame == null)
                    {
                        this.frame = frame;
                    }
                    else
                    {
                        this.frame = this.frame.Join(frame, "Symbol");
                    }
                }
                this.frame.Collect();
                this.columns = this.frame.Columns();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool dataCleaningForNulls()
        {
            try
            {

                IReadOnlyList<string> columns = frame.Columns();
                long numberOfRows = frame.Count();
                foreach (var column in columns)
                {
                    var percentNull = (this.frame.Filter(this.frame.Col(column.ToString()).IsNull()).Count() / numberOfRows) * 100;
                    if (percentNull > 30)
                    {
                        this.frame.Drop(column);
                    }
                    else if (this.frame.Col(column).GetType().Equals("String"))
                    {
                        this.frame.Drop(column);
                    }
                }
                foreach (var column in columns)
                {
                    if (this.frame.Col(column.ToString()).GetType().Equals("Integer"))
                    {
                        var obj = Functions.Mean(this.frame.Col(column));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }

        public bool filterRows(string column, string op, double value)
        {
            try
            {
                switch (op)
                {
                    case "<=":
                        this.frame = this.frame.Filter(this.frame.Col(column) <= value);
                        break;
                    case ">=":
                        this.frame = this.frame.Filter(this.frame.Col(column) >= value);
                        break;
                    case "not":
                        this.frame = this.frame.Filter(this.frame.Col(column) != value);
                        break;
                    case "<":
                        this.frame = this.frame.Filter(this.frame.Col(column) < value);
                        break;
                    case ">":
                        this.frame = this.frame.Filter(this.frame.Col(column) > value);
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
            return false;
        }

        public bool normalizeData()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public DataSetInfo prepareDataSet()
        {
            try
            {
                DataSetInfo dataSetInfo = new DataSetInfo();
                dataSetInfo.columns = this.columns.ToArray<string>();
                dataSetInfo.numberOfColumns = dataSetInfo.columns.Length;
                dataSetInfo.numberOfRows = (int)this.frame.Count();
                dataSetInfo.normalized = false;
                List<Row> rows = (List<Row>)this.frame.ToLocalIterator();
                foreach (var row in rows)
                {
                    dataSetInfo.dataSet.Add((string[])row.Values);
                }
                List<Row> summaryRows = (List<Row>)this.frame.Summary().ToLocalIterator();
                foreach (var row in summaryRows)
                {
                    dataSetInfo.dataSet.Add((string[])row.Values);
                }
                return dataSetInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public bool SentimentAnalysis()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> CreateDataViewForMLContext<T>()
        {
            try
            {
                dynamic result = null;
                string name = typeof(T).Name;
                switch (name)
                {
                    case "FinancialRatiosBarChart":
                        var resFinance = await this.database.cosmosClient.GetItemsFromCosmosDB<FinancialRatiosBarChart>("SELECT * FROM c");
                        result = resFinance.ToArray();
                        break;
                    case "GrowthBarChart":
                        var resGrowth = await this.database.cosmosClient.GetItemsFromCosmosDB<GrowthBarChart>("SELECT * FROM c");
                        result = resGrowth.ToArray();
                        break;
                    case "RatingsBarChart":
                        var resRating = await this.database.cosmosClient.GetItemsFromCosmosDB<RatingsBarChart>("SELECT * FROM c");
                        result = resRating.ToArray();
                        break;
                    case "TechnicalIndicatorsBarChart":
                        var resTechnical = await this.database.cosmosClient.GetItemsFromCosmosDB<RatingsBarChart>("SELECT * FROM c");
                        result = resTechnical.ToArray();
                        break;
                    default:
                        break;
                }
                PropertyInfo[] properties = result[0].GetType().GetProperties();
                var builder = new DataViewSchema.Builder();
                foreach (var prop in properties)
                {
                    if (prop.PropertyType.Name.Equals("String"))
                        continue;
                    if (prop.Name.Contains("IsMissing") || prop.Name.Contains("Replace") || prop.Name.Contains("Normalize"))
                    {
                        builder.AddColumn(prop.Name, BooleanDataViewType.Instance);
                        continue;
                    }

                    switch (prop.PropertyType.Name)
                    {
                        case "Integer":
                            builder.AddColumn(prop.Name, NumberDataViewType.Int32);
                            break;
                        case "Double":
                            builder.AddColumn(prop.Name, NumberDataViewType.Double);
                            break;
                        default:
                            break;
                    }
                }
                var schema = builder.ToSchema();
                this.dataView = mlContext.Data.LoadFromEnumerable(result, schema);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public IDataView PrepareDataSetForMLContext(PropertyInfo[] columns)
        {
            try
            {

                columns = columns.Where(x => !x.PropertyType.Name.Equals("String")).ToArray<PropertyInfo>();
                int dataSetRowCount = (int)this.dataView.GetRowCount();
                List<string> nullColumns = new List<string>();
                IDataView transformedData = null;
                foreach (var col in this.dataView.Schema)
                {
                    if (col.Type.RawType.Name.Equals("Double"))
                    {
                        MissingValueIndicatorEstimator indicatorEstimator = this.mlContext.Transforms.IndicateMissingValues($"IsMissing{col.Name}", col.Name);
                        MissingValueIndicatorTransformer missingValueTransFormer = indicatorEstimator.Fit(this.dataView);
                        transformedData = missingValueTransFormer.Transform((IDataView)this.dataView);
                        var column = transformedData.GetColumn<bool>($"IsMissing{col.Name}");
                        var rowCount = column.Count();
                        var MissingList = column.ToList<bool>();
                        var nullCount = MissingList.Where(x => x.Equals(true)).Count();
                        if (((nullCount / rowCount) * 100) > 30)
                        {
                            nullColumns.Add(col.Name);
                        }
                        else
                        {
                            var replacementEstimator = this.mlContext.Transforms.ReplaceMissingValues($"Replace{col.Name}", col.Name, replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean);
                            ITransformer replacementTransformer = replacementEstimator.Fit((IDataView)transformedData);
                            transformedData = replacementTransformer.Transform(transformedData);
                        }
                    }
                }
                if (nullColumns.Count() > 0)
                {
                    var dropColumnEstimator = this.mlContext.Transforms.DropColumns(nullColumns.ToArray());
                    var dropColumnTransformer = dropColumnEstimator.Fit((IDataView)transformedData);
                    transformedData = dropColumnTransformer.Transform((IDataView)transformedData);
                }
                foreach (var col in this.dataView.Schema)
                {
                    if (col.Type.RawType.Name.Equals("Double"))
                    {
                        var normalizeEstimator = this.mlContext.Transforms.NormalizeMinMax("Normalize" + col.Name, col.Name);
                        var normalizingTransformer = normalizeEstimator.Fit((IDataView)transformedData);
                        this.dataView = normalizingTransformer.Transform((IDataView)transformedData);
                    }
                }
                return this.dataView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public RegressionMetrics RunLinearRegression(string dependentVariable, string[] independentVariable, double partition)
        {
            try
            {

                var featureColumns = this.dataView.Schema.Where(x => x.Type.RawType.Name.Equals("Double")).Select(x => x.Name).ToArray<string>();
                foreach (var col in featureColumns)
                {
                    var pipeline = this.mlContext.Transforms.Conversion.ConvertType(col, col, DataKind.Single);
                    this.dataView = pipeline.Fit(this.dataView).Transform(this.dataView);
                }
                DataOperationsCatalog.TrainTestData dataSplit = this.mlContext.Data.TrainTestSplit(this.dataView, 0.3);
                IDataView trainData = dataSplit.TrainSet;
                IDataView testData = dataSplit.TestSet;
                IEstimator<ITransformer> dataPrepEstimator = this.mlContext.Transforms.Concatenate("Features", featureColumns);
                ITransformer trainingModelTransformer = dataPrepEstimator.Fit(trainData);
                SdcaRegressionTrainer sdcaTrainer = this.mlContext.Regression.Trainers.Sdca(labelColumnName: dependentVariable, featureColumnName: "Features");
                var trainedModel = sdcaTrainer.Fit(trainingModelTransformer.Transform(trainData));
                var trainedModelParameters = trainedModel.Model as LinearRegressionModelParameters;
                //test training data
                IDataView transformedTestData = trainingModelTransformer.Transform(testData);
                IDataView testDataPredictions = trainedModel.Transform(transformedTestData);
                RegressionMetrics trainedModelMetrics = this.mlContext.Regression.Evaluate(testDataPredictions);
                Console.WriteLine(trainedModelMetrics.RootMeanSquaredError);
                Console.WriteLine(trainedModelMetrics.RSquared);
                return trainedModelMetrics;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public KMeansClusteringModel RunKMeansClusteringModel(string featureColumnName, int numberOfClusters)
        {
            try
            {
            
                string[] features = this.dataView.Schema.Where(x => x.Type.RawType.Name.Equals("Double")).Select(y => y.Name).ToArray();
                IEstimator<ITransformer> dataPrepEstimator = this.mlContext.Transforms.Concatenate("Features", features);
                IDataView kmeansModelData = dataPrepEstimator.Fit(this.dataView).Transform(this.dataView);
                DataOperationsCatalog.TrainTestData dataSplit = this.mlContext.Data.TrainTestSplit(kmeansModelData, 0.3);
                IDataView trainData = dataSplit.TrainSet;
                IDataView testData = dataSplit.TestSet;
                KMeansTrainer trainer = this.mlContext.Clustering.Trainers.KMeans(featureColumnName, null, numberOfClusters);
                var modelData = trainer.Fit(trainData).Transform(testData);
                var metrics = this.mlContext.Clustering.Evaluate(modelData, "Label", "Score", "Features");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return new KMeansClusteringModel();
        }
    
    }
}

