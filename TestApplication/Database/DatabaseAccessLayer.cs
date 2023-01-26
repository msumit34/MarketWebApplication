using System.Data.SqlTypes;
using System.Data.Common;
using TestApplication.Database;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TestApplication
{
    public class DatabaseAccessLayer
    {
        internal SqlConnection connection { get; set; }
        private string connectionString = "";
        internal DatabaseCosmosDBClient cosmosClient; 

        public DatabaseAccessLayer()
        {
            JObject data = JObject.Parse(File.ReadAllText(@"C:\Users\malho\Source\Repos\msumit34\TestApplication\TestApplication\API.json"));
            this.connectionString = data["database_link"].ToString();
            this.connection = new SqlConnection(this.connectionString);
            cosmosClient = new DatabaseCosmosDBClient(data["cosmos_db_endpoint"].ToString(), data["cosmos_db_api_key"].ToString());
        }

        public bool OpenConnection()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public bool CloseConnection()
        {
            try
            {
                this.connection.Close();
                return true; 
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        private async Task<List<T>> readFromDatabase<T>(string query)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<List<T>> insertIntoDatabase<T>(string query)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
            return null;
        }

        public List<string> createSelectQueries<T>(string [] symbols)
        {
            try
            {
                List<string> queries = new List<string>();
                string baseQuery = $"SELECT * FROM {typeof(T).Name} WHERE Symbol = '";
                if (symbols.Length == 0)
                    return null;
                foreach (string symbol in symbols)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(baseQuery);
                    builder.Append($"{symbol}';");
                    queries.Add(builder.ToString());
                }
                return queries;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new List<string>();
        }

        public List<string> createQueries<T>(List<T> objList)
        {
            try
            {
                List<string> queries = new List<string>();
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (var obj in objList)
                {
                    StringBuilder query = new StringBuilder($"INSERT INTO {typeof(T).Name} VALUES(");
                    foreach (var prop in properties)
                    {
                        if (prop.PropertyType.Name.Equals("String"))
                        {
                            query.Append($"'{typeof(T).GetProperty(prop.Name).GetValue(obj)}',");
                        }
                        else if(prop.PropertyType.Name.Equals("Double") || prop.PropertyType.Name.Equals("Integer")) 
                        {
                            query.Append($"{typeof(T).GetProperty(prop.Name).GetValue(obj)},");
                        } 
                    }
                    string str = query.ToString();
                    str = str.TrimEnd(',');
                    str += ");";
                    queries.Add(str);
                }
                return queries;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
