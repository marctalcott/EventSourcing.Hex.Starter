using Domain.ES.Events;

namespace Domain.ES.Projections
{
    public interface IProjector
    {
        bool IsSubscribedTo(IEvent @event);

        string GetViewName(string streamId, IEvent @event);

        void Apply(IEvent @event, IView view);
    }
}