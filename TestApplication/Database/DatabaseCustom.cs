using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;

namespace TestApplication.Database
{
    public class DatabaseCustom : DatabaseAccessLayer
    {
        public DatabaseCustom() : base()
        { 
        
        }

        public async Task<Dictionary<string, dynamic>> customDatabaseQuery(string query)
        {
            try
            {
                this.OpenConnection();
                SqlDataReader reader = null; 
                Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                int counter = 0;
                using (SqlCommand command = new SqlCommand(query, this.connection)) 
                {
                    reader = await command.ExecuteReaderAsync(); 
                    while (reader.Read()) 
                    {
                        result.Add(reader.GetName(counter), reader.GetValue(counter));
                        counter++;
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
