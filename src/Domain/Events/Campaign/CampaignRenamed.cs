using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class CampaignRenamed: EventBase
    {
        public CampaignRenamed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
      
    }
}