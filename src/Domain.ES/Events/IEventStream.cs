using System.Collections.Generic;
using Domain.ES.Events;

namespace Domain.ES.EventStore
{
    public interface IEventStream
    {
        public string Id { get; }

        public int Version { get; }

        public IEnumerable<IEvent> Events { get; }
    }
}