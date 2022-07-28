using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Advertisers.Commands;
using Application.Campaigns.Commands;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using Xunit;

namespace Tests
{
    public class CampaignUnitTests : IEventTypeResolver
    {
        private TestConfig _testConfig = new TestConfig();

        public Type GetEventType(string typeName)
        {
            return Type.GetType($"Domain.Events.{typeName}, Domain");
        }

        [Fact]
        public async Task AddCampaignTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var repo = GetRepo();

            // add advertiser
            var addAdvertiser = new AddAdvertiser(userInfo, Guid.NewGuid(), "A test advertiser");
            var addAdvertiserHandler = new AddAdvertiserHandler(repo);
            await addAdvertiserHandler.Handle(addAdvertiser, CancellationToken.None);
            // add campaign
            var command = new AddCampaign(userInfo, Guid.NewGuid(), addAdvertiser.Id, "ABC Advertising Agency", null,
                null);
            var commandHandler = new AddCampaignHandler(repo);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Campaign>(command.Id);
            Assert.Equal(command.Id, testVal.Id);
            Assert.Equal(command.AdvertiserId, testVal.AdvertiserId);
            Assert.Equal(command.Name, testVal.Name);
        }

        [Fact]
        public async Task AddAndRenameAdvertiserTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var repo = GetRepo();

            // add advertiser
            var addAdvertiser = new AddAdvertiser(userInfo, Guid.NewGuid(), "A test advertiser");
            var addAdvertiserHandler = new AddAdvertiserHandler(repo);
            await addAdvertiserHandler.Handle(addAdvertiser, CancellationToken.None);
            // add campaign
            var command = new AddCampaign(userInfo, Guid.NewGuid(), addAdvertiser.Id, "EFG Advertising Agency", null,
                null);
            var commandHandler = new AddCampaignHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var rename = new RenameCampaign(userInfo, command.Id, "NewName Advertising Agency");
            var renameHandler = new RenameCampaignHandler(repo);
            await renameHandler.Handle(rename, CancellationToken.None);
            // Assert
            var testVal = await repo.Load<Campaign>(command.Id);
            Assert.Equal(rename.Id, testVal.Id);
            Assert.Equal(command.AdvertiserId, testVal.AdvertiserId);
            Assert.Equal(rename.Name, testVal.Name);
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
    }
}