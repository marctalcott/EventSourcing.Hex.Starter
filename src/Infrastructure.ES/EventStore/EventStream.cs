using System.Collections.Generic;
using System.Linq;
using Domain.ES.Events;

namespace Domain.ES.EventStore
{
    public class EventStream : IEventStream
    {
        private readonly List<IEvent> _events;

        public EventStream(string id, int version, IEnumerable<IEvent> events)
        {
            Id = id;
            Version = version;
            _events = events.ToList();
        }

        public string Id { get; private set; }

        public int Version { get; private set; }

        public IEnumerable<IEvent> Events
        {
            get { return _events; }
        }
    }
}