using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class CampaignDateRangeChanged: EventBase
    {
        public CampaignDateRangeChanged(Guid id, DateTime? startDate, DateTime? endDate)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guid Id { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
      
    }
}