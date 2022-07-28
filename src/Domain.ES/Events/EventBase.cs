using System;
using System.Diagnostics;

namespace Domain.ES.Events
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}