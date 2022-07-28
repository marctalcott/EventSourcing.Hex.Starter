using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Ads.Commands
{
    public class DeactivateAd : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }

        public DeactivateAd(EventUserInfo eventUserInfo, Guid id)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
        }
    }

    public class DeactivateAdHandler : IRequestHandler<ActivateAd>
    {
        private IRepository _repo;

        public DeactivateAdHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            ActivateAd command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var ad = await _repo.Load<Ad>(command.Id);
            ad.Activate();
            bool saved = await _repo.Save(command.EventUserInfo, ad);

            string aggName = "Ad";
            if (!saved)
                throw new ApplicationException($"{aggName} not added. Check your data and try again");
            return Unit.Value;
        }
    }
}