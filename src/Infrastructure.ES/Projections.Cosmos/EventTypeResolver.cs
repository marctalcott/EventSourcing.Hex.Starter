using System;
using Domain.ES.EventStore;

namespace Infrastructure.ES.Projections.Cosmos
{
    public class EventTypeResolver : IEventTypeResolver
    {
        public Type GetEventType(string typeName)
        {
            return Type.GetType($"Domain.Events.{typeName}, Domain");
        }
    }
}