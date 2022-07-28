using System;

namespace Domain.ES.EventStore
{
    public interface IEventTypeResolver
    {
        Type GetEventType(string typeName);
    }
}