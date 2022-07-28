using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Campaigns.Commands
{
    public class ChangeCampaignDateRange : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }

        public ChangeCampaignDateRange(
            EventUserInfo eventUserInfo,
            Guid id,
            DateTime? startDate,
            DateTime? endDate)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class ChangeCampaignDateRangeHandler : IRequestHandler<ChangeCampaignDateRange>
    {
        private IRepository _repo;

        public ChangeCampaignDateRangeHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            ChangeCampaignDateRange command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var campaign = await _repo.Load<Campaign>(command.Id);
            campaign.ChangeDateRange(command.StartDate, command.EndDate);
            bool saved = await _repo.Save(command.EventUserInfo, campaign);
            if (!saved)
                throw new ApplicationException($"{nameof(campaign)} not saved. Check your data and try again");
            return Unit.Value;
        }
    }
}