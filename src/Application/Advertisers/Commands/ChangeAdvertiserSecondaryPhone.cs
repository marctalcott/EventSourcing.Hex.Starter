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
    public class ChangeAdvertiserSecondaryPhone : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public PhoneNumber PhoneNumber { get; }

        public ChangeAdvertiserSecondaryPhone(EventUserInfo eventUserInfo, Guid id, PhoneNumber phoneNumber)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            PhoneNumber = phoneNumber;
        }
    }

    public class ChangeAdvertiserSecondaryPhoneHandler : IRequestHandler<ChangeAdvertiserSecondaryPhone>
    {
        private IRepository _repo;

        public ChangeAdvertiserSecondaryPhoneHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            ChangeAdvertiserSecondaryPhone command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var advertiser = await _repo.Load<Advertiser>(command.Id);
            advertiser.ChangeSecondaryPhone(command.PhoneNumber);
            bool saved = await _repo.Save(command.EventUserInfo, advertiser);
            if (!saved)
                throw new ApplicationException($"{nameof(advertiser)} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}