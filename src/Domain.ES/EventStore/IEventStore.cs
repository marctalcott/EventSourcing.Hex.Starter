using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ES.Events;

namespace Domain.ES.EventStore
{
    public interface IEventStore
    {
        Task<IEventStream> LoadStreamAsyncOrThrowNotFound(string streamId);
        Task<IEventStream> LoadStreamAsync(string streamId);

        Task<IEventStream> LoadStreamAsync(string streamId, int fromVersion);

        Task<bool> AppendToStreamAsync(
            EventUserInfo eventUserInfo,
            string streamId,
            int expectedVersion,
            IEnumerable<IEvent> events);
    }
}