using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;

namespace Infrastructure.ES.EventStore
{
    public interface IRepository
    {
        Task<T> Load<T>(Guid id)
            where T : AggregateBase;

        Task<T> Load<T>(string id)
            where T : AggregateBase;

        Task<bool> Save<T>(EventUserInfo eventUserInfo, T aggregate)
            where T : AggregateBase;
    }

    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<T> Load<T>(Guid id)
            where T : AggregateBase
        {
            return await Load<T>(id.ToString());
        }


        public async Task<T> Load<T>(string id)
            where T : AggregateBase
        {
            var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
            return (T)Activator.CreateInstance(typeof(T), stream.Events);
        }

        public async Task<bool> Save<T>(EventUserInfo eventUserInfo, T aggregate)
            where T : AggregateBase
        {
            if (eventUserInfo == null)
            {
                throw new ApplicationException("EventUserInfo was expected but not provided.");
            }

            if (!aggregate.Changes.Any())
                return true;

            var streamId = aggregate.Id;
            // save all events
            bool savedEvents = await _eventStore.AppendToStreamAsync(eventUserInfo,
                streamId.ToString(),
                aggregate.Version,
                aggregate.Changes);

            return savedEvents;
        }
    }
}