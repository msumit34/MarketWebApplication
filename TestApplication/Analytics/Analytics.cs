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

namespace TestApplication
{
    public class Analytics
    {
        private SparkSession session { get; set; }

        private DataFrame frame { get; set; }

        private IReadOnlyList<string> columns { get; set; }

        private List<DataFrame> frames { get; set; }

        public ModelObject dataView { get; set; }

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
            catch(Exception ex)
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
                switch(op)
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
                var result = await this.database.cosmosClient.GetItemsFromCosmosDB<T>("SELECT * FROM c");
                PropertyInfo[] properties = result[0].GetType().GetProperties();
                this.dataView = new ModelObject(result, typeof(T).GetProperties());
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public ModelObject PrepareDataSetForMLContext(PropertyInfo[] columns)
        {
            try
            {
                columns = columns.Where(x => !x.PropertyType.Name.Equals("String")).ToArray<PropertyInfo>();
                int dataSetRowCount = (int)this.dataView.GetRowCount();
                List<string> nullColumns = new List<string>();
                IDataView transformedData = null;
                foreach (var col in this.dataView.Schema)
                {
                    //MissingValueIndicatorEstimator indicatorEstimator = this.mlContext.Transforms.IndicateMissingValues($"IsNull{col.Name}", col.Name);
                    //MissingValueIndicatorTransformer missingValueTransFormer = indicatorEstimator.Fit(this.dataView);
                    //transformedData = missingValueTransFormer.Transform((IDataView)this.dataView);
                    //var column = transformedData.GetColumn<Boolean>($"IsNull{col.Name}");
                    var replacementEstimator = this.mlContext.Transforms.ReplaceMissingValues($"Replaced{col.Name}", col.Name, replacementMode: Microsoft.ML.Transforms.MissingValueReplacingEstimator.ReplacementMode.Mean);
                    ITransformer replacementTransformer = replacementEstimator.Fit((IDataView)this.dataView);
                    transformedData = replacementTransformer.Transform(this.dataView);
                }
                var dropColumnEstimator = this.mlContext.Transforms.DropColumns(nullColumns.ToArray());
                var dropColumnTransformer = dropColumnEstimator.Fit((IDataView)transformedData);
                this.dataView = (ModelObject)dropColumnTransformer.Transform((IDataView)transformedData);
                var modelColumns = columns.Select(x => x.Name).Except(nullColumns).ToArray<string>();
                foreach (var col in modelColumns)
                {
                    var normalizeEstimator = this.mlContext.Transforms.NormalizeMinMax("normalized" + col, col);
                    var normalizingTransformer = normalizeEstimator.Fit((IDataView)transformedData);
                    this.dataView = (ModelObject)normalizingTransformer.Transform((IDataView)transformedData);
                }
                return this.dataView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
         
        public RegressionMetrics RunLinearRegression(string dependentVariable, string[] independentVariable, int[] partition)
        {
            try
            {
                this.dataCleaningForNulls();
                MLContext mlContext = new MLContext();
                SdcaRegressionTrainer trainer = mlContext.Regression.Trainers.Sdca();
                var trainedModel = trainer.Fit((IDataView)this.dataView);
                IDataView predictions = trainedModel.Transform((IDataView)this.dataView);
                RegressionMetrics trainedModelMetrics = this.mlContext.Regression.Evaluate(predictions);
                return trainedModelMetrics;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public KMeansClusteringModel RunKMeansClusteringModel(string[] features, int numberOfClusters)
        {
            try
            {
                MLContext mlContext = new MLContext();
                KMeansTrainer trainer = mlContext.Clustering.Trainers.KMeans(features[0], null, numberOfClusters);
                trainer.Fit(null);
                return new KMeansClusteringModel();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return new KMeansClusteringModel();
        }
    }


    public class ModelObject : IDataView
    {
        public bool CanShuffle => false;

        public DataViewSchema Schema { get; }
        public IEnumerable _data { get; }

        public ModelObject()
        {

        }

        public ModelObject(IEnumerable data, PropertyInfo[] props)
        {
            var builder = new DataViewSchema.Builder();
            this._data = data;
            PropertyInfo[] properties = props;
            foreach (var prop in properties)
            {
                if (prop.PropertyType.Name.Equals("String"))
                    continue;
                switch (prop.PropertyType.Name)
                {
                    case "Integer":
                        builder.AddColumn(prop.Name, NumberDataViewType.Int32);
                        builder.AddColumn($"Replaced{prop.Name}", NumberDataViewType.Int32);
                        break;
                    case "Double":
                        builder.AddColumn(prop.Name, NumberDataViewType.Double);
                        builder.AddColumn($"Replaced{prop.Name}", NumberDataViewType.Double);
                        break;
                    default:
                        break;
                }
            }
            this.Schema = builder.ToSchema();
        }

        public long? GetRowCount()
        {
            long value = 0;
            if (_data == null)
                return 0;
            IEnumerator enumerator = _data.GetEnumerator();
            while (enumerator.MoveNext())
            {
                value = value + 1;
            }
            return value;
        }

        public DataViewRowCursor GetRowCursor(IEnumerable<DataViewSchema.Column> columnsNeeded, Random rand = null)
        {
            try
            {
                return new Cursor(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public DataViewRowCursor[] GetRowCursorSet(IEnumerable<DataViewSchema.Column> columnsNeeded, int n, Random rand = null)
        {
            try
            {
                return new[] { GetRowCursor(columnsNeeded, rand) };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        private sealed class Cursor : DataViewRowCursor
        {
            private bool _disposed;
            private long _position;
            private readonly IEnumerator<ModelObject> _enumerator;
            private Delegate _getters =()=> { Console.WriteLine("Test"); };

            public override long Position => _position;
            public override long Batch => 0;
            public override DataViewSchema Schema { get; }
            public ModelObject ModelObject { get; }

            public Cursor(IEnumerator<ModelObject> parent)
            {
                Schema = parent.Current.Schema;
                _position = -1;
                _enumerator = parent;

            }
            public Cursor(ModelObject modelObject)
            {
                ModelObject = modelObject;
                Schema = modelObject.Schema;
                _position = -1;
                IEnumerable<ModelObject> enummerable = new List<ModelObject>() { modelObject };
                _enumerator = enummerable.GetEnumerator();
                this._getters = (string columnName) => { Console.WriteLine("text"); };
            }

            protected override void Dispose(bool disposing)
            {
                if (_disposed)
                    return;
                if (disposing)
                {
                    _enumerator.Dispose();
                    _position = -1;
                }
                _disposed = true;
                base.Dispose(disposing);
            }


            public override ValueGetter<TValue> GetGetter<TValue>(
                DataViewSchema.Column column)

            {
                if (!IsColumnActive(column))
                    throw new ArgumentOutOfRangeException(nameof(column));
                return (ValueGetter<TValue>)_getters;
            }

            public override bool IsColumnActive(DataViewSchema.Column column)
                => ModelObject.GetColumn<dynamic>(column) != null;

            public override bool MoveNext()
            {
                if (_disposed)
                    return false;
                if (_enumerator.MoveNext())
                {
                    _position++;
                    return true;
                }
                Dispose();
                return false;
            }

            public override ValueGetter<DataViewRowId> GetIdGetter()
            {
                throw new NotImplementedException();
            }
        }
    }
}

