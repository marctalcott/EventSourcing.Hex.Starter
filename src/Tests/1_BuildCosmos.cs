using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Xunit;

namespace Tests
{
    public class BuildCosmos
    {
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task SC00_MigrateDB()
        {
            var client = new CosmosClient(_testConfig.EndpointUrl, _testConfig.AuthorizationKey);

            await client.CreateDatabaseIfNotExistsAsync(_testConfig.DatabaseId,
                ThroughputProperties.CreateManualThroughput(400));
            Database database = client.GetDatabase(_testConfig.DatabaseId);

            await database.DefineContainer("events", "/stream/id").CreateIfNotExistsAsync();
            await database.DefineContainer("leases", "/id").CreateIfNotExistsAsync();
            await database.DefineContainer("views", "/id").CreateIfNotExistsAsync();
            //await database.DefineContainer("snapshots", "/id").CreateIfNotExistsAsync();

            // await database.DefineContainer("fail", "/stream/id").CreateIfNotExistsAsync();
            // await database.DefineContainer("pass", "/stream/id").CreateIfNotExistsAsync();
            // await database.DefineContainer("newevents", "/stream/id").CreateIfNotExistsAsync();

            StoredProcedureProperties storedProcedureProperties = new StoredProcedureProperties
            {
                Id = "spAppendToStream",
                Body = File.ReadAllText("js/spAppendToStream.js")
            };


            var eventContainerNames = new[] { "events" };
            foreach (var containerName in eventContainerNames)
            {
                Container eventsContainer = database.GetContainer(containerName);
                try
                {
                    await eventsContainer.Scripts.DeleteStoredProcedureAsync("spAppendToStream");
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // Stored procedure didn't exist yet.
                }

                await eventsContainer.Scripts.CreateStoredProcedureAsync(storedProcedureProperties);
            }
        }
    }
}