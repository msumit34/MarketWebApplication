using System.Data.SqlClient;
using System.Reflection;

namespace TestApplication.Database
{
    public class DatabaseRead : DatabaseAccessLayer
    {

        public DatabaseRead():base()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> ReadFromCosmosDBAsync<T>(string id)
        {
            try
            {
                var result = await this.cosmosClient.GetItemFromCosmosDB<T>(id);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default(T);
        }

        public async Task<List<T>> readDatabase<T>(string[] query)
        {
            try
            {
                this.OpenConnection();
                List<string> queries = this.createSelectQueries<T>(query);
                List<T> objList = new List<T>();
                PropertyInfo[] properties = typeof(T).GetProperties();
                SqlCommand command = null;
                foreach (string q in queries)
                {
                    command = new SqlCommand(q, this.connection);
                    command.CommandTimeout = 99999999;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T obj = (T)Activator.CreateInstance(typeof(T));
                        foreach (var prop in properties)
                        {
                            obj.GetType().GetProperty(prop.Name).SetValue(obj, reader.GetValue(reader.GetOrdinal(prop.Name)));
                        }
                        objList.Add(obj);
                    }
                }
                return objList;
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}
