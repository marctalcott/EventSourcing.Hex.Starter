using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Domain.ES.Exceptions;
using Infrastructure.ES.EventStore;
using MediatR;


namespace Application.Campaigns.Commands
{
    public class AddCampaign : IRequest
    { 
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }   
        public Guid AdvertiserId { get; }
        public string Name { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
        
        public AddCampaign(EventUserInfo eventUserInfo, 
            Guid id, 
            Guid advertiserId, 
            string name, 
            DateTime? startDate, 
            DateTime? endDate)
        { 
            EventUserInfo = eventUserInfo;
            Id = id;
            AdvertiserId = advertiserId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
  
    public class AddCampaignHandler : IRequestHandler<AddCampaign>
    {
        private IRepository _repo;

        public AddCampaignHandler(IRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<Unit> Handle(
            AddCampaign command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new AppException("User must be defined.");

            // make sure that advertiser exists.
            try
            {
                await _repo.Load<Advertiser>(command.AdvertiserId);
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("The advertiser does not exist.");
            }

            var campaign = new Campaign(command.Id, command.AdvertiserId, command.Name, command.StartDate, command.EndDate);
            bool saved = await _repo.Save(command.EventUserInfo, campaign);
            if (!saved)
                throw new AppException($"{nameof(campaign)} not added.  Was the Id a unique value?");
            return Unit.Value;
        }
    }
}