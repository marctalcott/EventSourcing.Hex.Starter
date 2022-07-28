using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Advertisers.Commands;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Domain.ValueObjects;
using Infrastructure.ES.EventStore;
using Xunit;

namespace Tests
{
    public class AdvertiserUnitTests : IEventTypeResolver
    {
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