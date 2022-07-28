using Domain.ES.Events;
using Infrastructure.ES.EventStore;

namespace Utility.CosmosValidator
{
    public interface IValidator
    {
        bool IsSubscribedTo(IEvent @event);

        EventWrapper GetExpectedValue(IEvent e, EventWrapper eventWrapper);
    }
}