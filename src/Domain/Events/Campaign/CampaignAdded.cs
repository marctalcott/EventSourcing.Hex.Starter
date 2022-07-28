using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class CampaignAdded: EventBase
    {
        public CampaignAdded(Guid id, Guid advertiserId, string name, DateTime? startDate, DateTime? endDate)
        {
            Id = id;
            AdvertiserId = advertiserId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guid Id { get; }
        public Guid AdvertiserId { get; }
        public string Name { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
      
    }
}