using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Campaigns.Commands
{
    public class RenameCampaign : IRequest
    {
        public Guid Id { get; }
        public EventUserInfo EventUserInfo { get; }
        public string Name { get; }


        public RenameCampaign(EventUserInfo eventUserInfo, Guid id, string name)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Name = name;
        }
    }

    public class RenameCampaignHandler : IRequestHandler<RenameCampaign>
    {
        private IRepository _repo;

        public RenameCampaignHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            RenameCampaign command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var campaign = await _repo.Load<Campaign>(command.Id);
            campaign.Rename(command.Name);
            bool saved = await _repo.Save(command.EventUserInfo, campaign);
            if (!saved)
                throw new ApplicationException($"{nameof(campaign)} not saved. Check your data and try again");
            return Unit.Value;
        }
    }
}