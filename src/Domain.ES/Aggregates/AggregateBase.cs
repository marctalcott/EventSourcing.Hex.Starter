

using Domain.ES.Events;

namespace Domain.Aggregates
{
    public abstract class AggregateBase
    {
        public  Guid Id { get; protected set; }
        public int Version { get; protected set; }
        public List<IEvent> Changes { get; } = new List<IEvent>();
        protected virtual void Apply(IEvent @event)
        {
            Changes.Add(@event);
            Mutate(@event);
        }

        protected abstract void Mutate(IEvent @event);
        
        protected void InitFromEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Mutate(@event);
                Version += 1;
            }
        }
    }
}