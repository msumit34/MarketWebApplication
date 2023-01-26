using System.Data.SqlClient;

namespace TestApplication.Database
{
    public class DatabaseWrite : DatabaseAccessLayer
    {
        public DatabaseWrite() :base()
        { 
        
        }

        public async Task<bool> writeToCosmosDB<T>(List<T> list)
        {
            try
            {
                foreach (var l in list)
                {
                    typeof(T).GetProperty("type").SetValue(l, typeof(T).Name);
                    string id =  l.GetType().GetProperty("Value").GetValue(l).ToString() + "_" + DateTime.Now.ToString("yyyy_MM_dd") + "_" + 
                                 l.GetType().GetProperty("type").GetValue(l).ToString();
                    this.cosmosClient.CreateItemInCosmosDB<T>(l, id);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task<bool> writeToCosmosDB<T>(List<T> list, string collectionName)
        {
            try
            {
                foreach (var l in list)
                {
                    string id = l.GetType().GetProperty("Value").GetValue(l).ToString() + "_" + DateTime.Now.ToString("yyyy_MM_dd") + "_" +
                                 l.GetType().GetProperty("type").GetValue(l).ToString();
                    this.cosmosClient.CreateItemInCosmosDB<T>(l, id, collectionName);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }


        public async Task<bool> writeToDatabase<T>(List<T> list)
        {
            try
            {
                List<string> queries = this.createQueries<T>(list);
                foreach(var query in queries)
                {
                    using (SqlCommand command = new SqlCommand(query, this.connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

    }
}
