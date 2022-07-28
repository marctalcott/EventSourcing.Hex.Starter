using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Advertisers.Commands;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Domain.ES.Projections;
using Domain.Projections;
using Domain.ValueObjects;
using Infrastructure.ES.EventStore;
using Infrastructure.ES.Projections.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using Xunit;

namespace Port.A_Demo
{
    public class DemoBasicTests : IEventTypeResolver
    {
        private const string EndpointUrl = "https://esdemomarc.documents.azure.com:443/";

        private static readonly string AuthorizationKey =
            "p02PIPOR2O7czz9jOk7MDW4yEV0BcQ6i5WZmERJq1LKQXvvOGgEpPdCuuL5rfazCXE0BI8Ww5In08S6FQA6ruA==";

        //  Environment.GetEnvironmentVariable("COSMOSDB_EVENT_SOURCING_KEY");
        private const string DatabaseId = "esdemo2";
        private const string EventContainer = "events";

        public Type GetEventType(string typeName)
        {
            return Type.GetType($"Domain.Events.{typeName}, Domain");
        }

        [Fact]
        public async Task Engine1_RunCosmosProjectionsAsync()
        {
            IViewRepository viewRepository = new CosmosViewRepository(EndpointUrl, AuthorizationKey, DatabaseId);
            IProjectionEngine projectionEngine =
                new CosmosProjectionEngine(this, viewRepository, EndpointUrl, AuthorizationKey, DatabaseId);

            projectionEngine.RegisterProjector(new AdvertiserProjector());

            await projectionEngine.StartAsync("TestInstance");

            await Task.Delay(-1);
        }


        [Fact]
        public async Task AddAdvertiserTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "ABC Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(command.Id, testVal.Id);
            Assert.Equal(command.Name, testVal.Name);
        }

        [Fact]
        public async Task AddAndRenameAdvertiserTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "EFG Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var rename = new RenameAdvertiser(userInfo, command.Id, "NewName Advertising Agency");
            var renameHandler = new RenameAdvertiserHandler(repo);
            await renameHandler.Handle(rename, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(rename.Id, testVal.Id);
            Assert.Equal(rename.Name, testVal.Name);
        }

        [Fact]
        public async Task ChangeAdvertiserEmailAddressTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "EFG Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var changeEmail = new ChangeAdvertiserEmailAddress(userInfo,
                command.Id,
                new EmailAddress("test@aol.com"));
            var changeEmailHandler = new ChangeAdvertiserEmailAddressHandler(repo);
            await changeEmailHandler.Handle(changeEmail, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(changeEmail.Id, testVal.Id);
            Assert.Equal(changeEmail.EmailAddress, testVal.EmailAddress);
        }

        [Fact]
        public async Task ChangeAdvertiserAddressTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "EFG Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var changeAddress = new ChangeAdvertiserAddress(userInfo,
                command.Id,
                new Address("123 Main Street",
                    "",
                    "",
                    "Greensboro",
                    "NC",
                    "USA",
                    "27313"));

            var changeAddressHandler = new ChangeAdvertiserAddressHandler(repo);
            await changeAddressHandler.Handle(changeAddress, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(changeAddress.Id, testVal.Id);
            Assert.Equal(changeAddress.Address, testVal.Address);
        }

        [Fact]
        public async Task ChangeAdvertiserPrimaryPhoneTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "EFG Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var changePrimaryPhone = new ChangeAdvertiserPrimaryPhone(userInfo,
                command.Id,
                new PhoneNumber("3335557788", PhoneType.Mobile));

            var changePhoneHandler = new ChangeAdvertiserPrimaryPhoneHandler(repo);
            await changePhoneHandler.Handle(changePrimaryPhone, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(changePrimaryPhone.Id, testVal.Id);
            Assert.Equal(changePrimaryPhone.PhoneNumber, testVal.PrimaryPhoneNumber);
        }

        [Fact]
        public async Task ChangeAdvertiserSecondaryPhoneTest()
        {
            // Arrange
            var userInfo = new EventUserInfo("test|d324dadae3", false);
            var command = new AddAdvertiser(userInfo, Guid.NewGuid(), "EFG Advertising Agency");
            var repo = GetRepo();
            var commandHandler = new AddAdvertiserHandler(repo);
            await commandHandler.Handle(command, CancellationToken.None);

            // act
            var changeSecondaryPhone = new ChangeAdvertiserSecondaryPhone(userInfo,
                command.Id,
                new PhoneNumber("3335557788", PhoneType.Mobile));

            var changePhoneHandler = new ChangeAdvertiserSecondaryPhoneHandler(repo);
            await changePhoneHandler.Handle(changeSecondaryPhone, CancellationToken.None);

            // Assert
            var testVal = await repo.Load<Advertiser>(command.Id);
            Assert.Equal(changeSecondaryPhone.Id, testVal.Id);
            Assert.Equal(changeSecondaryPhone.PhoneNumber, testVal.PrimaryPhoneNumber);
        }

        private IRepository GetRepo()
        {
            var eventStore = new CosmosDBEventStore(this, EndpointUrl, AuthorizationKey, DatabaseId, EventContainer);
            var repo = new Repository(eventStore);
            return repo;
        }

        [Fact]
        public async Task SC00_MigrateDB()
        {
            var client = new CosmosClient(EndpointUrl, AuthorizationKey);

            await client.CreateDatabaseIfNotExistsAsync(DatabaseId, ThroughputProperties.CreateManualThroughput(400));
            Database database = client.GetDatabase(DatabaseId);

            await database.DefineContainer("events", "/stream/id").CreateIfNotExistsAsync();
            await database.DefineContainer("leases", "/id").CreateIfNotExistsAsync();
            await database.DefineContainer("views", "/id").CreateIfNotExistsAsync();
            await database.DefineContainer("snapshots", "/id").CreateIfNotExistsAsync();

            await database.DefineContainer("fail", "/stream/id").CreateIfNotExistsAsync();
            await database.DefineContainer("pass", "/stream/id").CreateIfNotExistsAsync();
            await database.DefineContainer("newevents", "/stream/id").CreateIfNotExistsAsync();

            StoredProcedureProperties storedProcedureProperties = new StoredProcedureProperties
            {
                Id = "spAppendToStream",
                Body = File.ReadAllText("js/spAppendToStream.js")
            };

            var containerNames = new[] { "events", "newevents", "pass", "fail" };
            foreach (var containerName in containerNames)
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