using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Advertisers.Commands
{
    public class RenameAdvertiser : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public string Name { get; }


        public RenameAdvertiser(EventUserInfo eventUserInfo, Guid id, string name)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Name = name;
        }
    }

    public class RenameAdvertiserHandler : IRequestHandler<RenameAdvertiser>
    {
        private IRepository _repo;

        public RenameAdvertiserHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            RenameAdvertiser command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var advertiser = await _repo.Load<Advertiser>(command.Id);
            advertiser.Rename(command.Name);
            bool saved = await _repo.Save(command.EventUserInfo, advertiser);
            if (!saved)
                throw new ApplicationException($"{nameof(advertiser)} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}