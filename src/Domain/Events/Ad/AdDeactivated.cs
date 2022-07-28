using System;
using Domain.ES.Events;

namespace Domain.Events
{
    public class AdDeactivated: EventBase
    {
        public AdDeactivated(Guid id)
        {
            Id = id;
        }
 
        public Guid Id { get; } 
    }
}