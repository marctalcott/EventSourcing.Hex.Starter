using System;
using System.Collections.Generic;
using Domain.ES.Events;
using Newtonsoft.Json.Linq;

namespace Domain.ES.Projections
{
    public abstract class Projector<TView>
        where TView : new()
    {
        private readonly Dictionary<Type, Action<IEvent, object>> _handlers;

        public Projector()
        {
            _handlers = new Dictionary<Type, Action<IEvent, object>>();
        }

        public bool IsSubscribedTo(IEvent @event) => _handlers.ContainsKey(@event.GetType());

        public virtual string GetViewName(string streamId, IEvent @event) => typeof(TView).Name;

        public void Apply(IEvent @event, IView view)
        {
            var payload = view.Payload.ToObject<TView>()!;

            var eventType = @event.GetType();
            if (_handlers.TryGetValue(eventType, out Action<IEvent, object> handler))
            {
                if (handler == null)
                {
                    throw new ApplicationException($"Could not get handler for {eventType}");
                }

                handler(@event, payload);

                view.Payload = JObject.FromObject(payload);
            }
        }

        protected void RegisterHandler<TEvent>(Action<TEvent, TView> handler)
            where TEvent : IEvent
        {
            _handlers[typeof(TEvent)] = new Action<IEvent, object>((e, v) => handler((TEvent)e, (TView)v));
        }
    }
}