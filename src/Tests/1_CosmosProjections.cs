using System;
using System.Threading.Tasks;
using Domain.ES.EventStore;
using Domain.ES.Projections;
using Domain.Projections;
using Infrastructure.ES.Projections.Cosmos;
using Xunit;

namespace Tests
{
    public class CosmosProjections : IEventTypeResolver
    {
        private TestConfig _testConfig = new TestConfig();

        public Type GetEventType(string typeName)
        {
            return Type.GetType($"Domain.Events.{typeName}, Domain");
        }

        [Fact]
        public async Task Engine1_RunCosmosProjectionsAsync()
        {
            IViewRepository viewRepository = new CosmosViewRepository(_testConfig.EndpointUrl,
                _testConfig.AuthorizationKey, _testConfig.DatabaseId);
            IProjectionEngine projectionEngine =
                new CosmosProjectionEngine(this, viewRepository, _testConfig.EndpointUrl, _testConfig.AuthorizationKey,
                    _testConfig.DatabaseId);

            projectionEngine.RegisterProjector(new AdvertiserProjector());


            await projectionEngine.StartAsync("TestInstance");

            await Task.Delay(-1);
        }
    }
}