using System;

namespace Domain.ES.Events
{
    public interface IEvent
    {
        public DateTime Timestamp { get; set; }
    }
}