using Microsoft.Azure.Cosmos;


namespace TestApplication.Database
{
    public class DatabaseCosmosDBClient
    {
        private string endpoint { get; set; }
        private string private_key { get; set; }
        private CosmosClient client { get; set; }

        private string DatabaseName { get; set; }
        
        public DatabaseCosmosDBClient(string endpoint, string private_key)
        {
            this.client = new CosmosClient(endpoint, private_key);
            this.DatabaseName = "SecuritiesDB";
        }

        public async Task<bool> CreateItemInCosmosDB<T>(T item, string id)
        {
            try
            {
                Container container = this.client.GetContainer(this.DatabaseName, typeof(T).Name);
                typeof(T).GetProperty("id").SetValue(item,id);
                await container.CreateItemAsync<T>(
                    item: item,
                    partitionKey: new PartitionKey(id)
                ); 
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task<bool> CreateItemInCosmosDB<T>(T item, string id, string containerName)
        {
            try
            {
                Container container = this.client.GetContainer(this.DatabaseName, containerName);
                typeof(T).GetProperty("id").SetValue(item, id);
                await container.CreateItemAsync<T>(
                    item: item,
                    partitionKey: new PartitionKey(id)
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public async Task<List<T>> GetItemsFromCosmosDB<T>(string sql, string containerName)
        {
            try
            {
                string containerValue = "";
                if (!containerName.Equals(""))
                    containerValue = containerName;
                else
                    containerValue = typeof(T).Name;
                List<T> listOfItems = new List<T>();
                Container container = this.client.GetContainer(this.DatabaseName, containerValue);
                FeedIterator<T> iterator = container.GetItemQueryIterator<T>(sql, requestOptions: new QueryRequestOptions { MaxConcurrency = 5, MaxItemCount = 1000 });
                foreach (var iter in await iterator.ReadNextAsync())
                {
                    listOfItems.Add(iter);
                }
                return listOfItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public async Task<List<T>> GetItemsFromCosmosDB<T>(string sql)
        {
            try
            {
                List<T> listOfItems = new List<T>();
                Container container = this.client.GetContainer(this.DatabaseName, typeof(T).Name);
                FeedIterator<T> iterator = container.GetItemQueryIterator<T>(sql, requestOptions: new QueryRequestOptions{MaxConcurrency = 2, MaxItemCount = 100 });
                foreach(var iter in await iterator.ReadNextAsync())
                {
                    listOfItems.Add(iter);
                }
                return listOfItems;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public async Task<T> GetItemFromCosmosDB<T>(string id)
        {
            try
            {
                Container container = this.client.GetContainer(this.DatabaseName, typeof(T).Name);
                T item = await container.ReadItemAsync<T>(
                    id: id, 
                    partitionKey: new PartitionKey(id)
                );
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return default(T);
        }

        public async Task<bool> ReplaceItemInCosmosDB<T>(string id, T item)
        {
            try
            {
                Container container = client.GetContainer(this.DatabaseName, typeof(T).Name);
                await container.ReplaceItemAsync<T>(
                    item: item,
                    id: id,
                    partitionKey: new PartitionKey(id)
                ); 
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> DeleteItemInCosmosDB<T>()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

    }
}
