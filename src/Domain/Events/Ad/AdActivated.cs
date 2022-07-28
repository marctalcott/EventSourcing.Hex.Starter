using System;
using Domain.ES.Events;

namespace Domain.Events
{
    public class AdActivated:EventBase
    {
        public AdActivated(Guid id)
        {
            Id = id;
        }
 
        public Guid Id { get; } 
    }
}