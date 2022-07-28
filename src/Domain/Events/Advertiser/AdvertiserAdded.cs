using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class AdvertiserAdded: EventBase
    {
        public AdvertiserAdded(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
      
    }
}