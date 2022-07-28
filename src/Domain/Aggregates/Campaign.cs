using System;
using System.Collections.Generic;
using Domain.ES.Events;
using Domain.Events;

namespace Domain.Aggregates
{
    public class Campaign : AggregateBase
    {
        public Guid AdvertiserId { get; set; }
        public string Name { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public Campaign(IEnumerable<IEvent> events)
        {
            InitFromEvents(events);
        }

        public Campaign(Guid id, Guid advertiserId, string name, DateTime? startDate, DateTime? endDate)
        {
            Apply(new CampaignAdded(id, advertiserId, name, startDate, endDate));
        }

        public void Rename(string name)
        {
            if (this.Name != name)
            {
                Apply(new CampaignRenamed(this.Id, name));
            }
        }

        public void ChangeDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (this.StartDate != startDate
                ||  this.EndDate != endDate)
                
            {
                Apply(new CampaignDateRangeChanged(this.Id, this.StartDate, this.EndDate));
            }
        }
     
        protected override void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }

        protected void When(CampaignAdded command)
        {
            Id = command.Id;
            AdvertiserId = command.AdvertiserId;
            Name = command.Name;
            StartDate = command.StartDate;
            EndDate = command.EndDate;
        }

        protected void When(CampaignRenamed command)
        {
            this.Name = command.Name;
        }

        protected void When(CampaignDateRangeChanged command)
        {
            this.StartDate = command.StartDate;
            this.EndDate = command.EndDate;
        }
        
    }
}