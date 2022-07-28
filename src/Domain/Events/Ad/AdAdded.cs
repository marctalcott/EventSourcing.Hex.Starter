using System;
using Domain.Enums;
using Domain.ES.Events;

namespace Domain.Events
{
    public class AdAdded : EventBase
    {
        public AdAdded(Guid id, string name, AdStatus adStatus, bool active)
        {
            Id = id;
            Name = name;
            AdStatus = adStatus;
            Active = active;
        }

        public Guid Id { get; }

        public string Name { get; }
        public AdStatus AdStatus { get; }
        public bool Active { get; }
    }
}