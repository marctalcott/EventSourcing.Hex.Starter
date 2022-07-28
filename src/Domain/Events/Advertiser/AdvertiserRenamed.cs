using System;
using Domain.ES.Events;

namespace Domain.Events
{
    public class AdvertiserRenamed:EventBase
    {
        public AdvertiserRenamed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
      
    }
}