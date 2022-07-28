using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Ads.Commands
{
    public class RenameAd : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public string Name { get; }


        public RenameAd(EventUserInfo eventUserInfo, Guid id, string name)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Name = name;
        }
    }

    public class RenameAdHandler : IRequestHandler<RenameAd>
    {
        private IRepository _repo;

        public RenameAdHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            RenameAd command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var ad = await _repo.Load<Ad>(command.Id);
            ad.Rename(command.Name);
            bool saved = await _repo.Save(command.EventUserInfo, ad);
            string aggName = "Ad";
            if (!saved)
                throw new ApplicationException($"{aggName} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}