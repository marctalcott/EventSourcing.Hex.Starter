using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Advertisers.Commands;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Domain.Projections;
using Infrastructure.ES.EventStore;
using Infrastructure.ES.Projections.Cosmos;
using Xunit;

namespace Tests
{
    public class AdvertiserProjectionTests : IEventTypeResolver
    {
        private int delayMs = 6000;
        private TestConfig _testConfig = new TestConfig();

        public Type GetEventType(string typeName)
        {
            return Type.GetType($"Domain.Events.{typeName}, Domain");
        }

        [Fact]
        public async Task AddAdvertiserTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var id = Guid.NewGuid();
            var command = new AddAdvertiser(userInfo, id, "ABC Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);
            await Task.Delay(delayMs);

            // now some time goes by so we pull it from EventStore
            var viewRepo = this.GetViewRepository();
            string viewName = $"AdvertiserView:{id}";
            var view = await viewRepo.LoadTypedViewAsync<AdvertiserView>(viewName);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(command.Id, testVal.Id);
            Assert.Equal(command.Name, testVal.Name);
        }

        private IRepository GetRepo()
        {
            var eventStore = new CosmosDBEventStore(this,
                _testConfig.EndpointUrl,
                _testConfig.AuthorizationKey,
                _testConfig.DatabaseId,
                _testConfig.EventsContainer);
            var repo = new Repository(eventStore);
            return repo;
        }

        private CosmosViewRepository GetViewRepository()
        {
            return new CosmosViewRepository(_testConfig.EndpointUrl,
                _testConfig.AuthorizationKey,
                _testConfig.DatabaseId,
                _testConfig.ViewsContainer);
        }
    }
}