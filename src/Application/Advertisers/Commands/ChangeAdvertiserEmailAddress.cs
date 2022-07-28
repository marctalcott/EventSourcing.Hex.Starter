using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Domain.ValueObjects;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Advertisers.Commands
{
    public class ChangeAdvertiserEmailAddress : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public EmailAddress EmailAddress { get; }

        public ChangeAdvertiserEmailAddress(EventUserInfo eventUserInfo, Guid id, EmailAddress emailAddress)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            EmailAddress = emailAddress;
        }
    }

    public class ChangeAdvertiserEmailAddressHandler : IRequestHandler<ChangeAdvertiserEmailAddress>
    {
        private IRepository _repo;

        public ChangeAdvertiserEmailAddressHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            ChangeAdvertiserEmailAddress command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var advertiser = await _repo.Load<Advertiser>(command.Id);
            advertiser.ChangeEmailAddress(command.EmailAddress);
            bool saved = await _repo.Save(command.EventUserInfo, advertiser);
            
            
            if (!saved)
                throw new ApplicationException($"{nameof(advertiser)} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}