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
    public class ChangeAdvertiserAddress : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public Address Address { get; }


        public ChangeAdvertiserAddress(EventUserInfo eventUserInfo, Guid id, Address address)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Address = address;
        }
    }

    public class ChangeAdvertiserAddressHandler : IRequestHandler<ChangeAdvertiserAddress>
    {
        private IRepository _repo;

        public ChangeAdvertiserAddressHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            ChangeAdvertiserAddress command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var advertiser = await _repo.Load<Advertiser>(command.Id);
            advertiser.ChangeAddress(command.Address);
            bool saved = await _repo.Save(command.EventUserInfo, advertiser);
            string aggName = nameof(advertiser);
            if (!saved)
                throw new ApplicationException($"{aggName} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}