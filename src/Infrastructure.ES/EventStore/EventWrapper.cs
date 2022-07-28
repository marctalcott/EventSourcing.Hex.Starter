using System;
using Domain.ES.Events;
using Domain.ES.EventStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.ES.EventStore
{
    public class EventWrapper
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("stream")] public StreamInfo StreamInfo { get; set; }

        [JsonProperty("eventType")] public string EventType { get; set; }

        [JsonProperty("eventData")] public JObject EventData { get; set; }

        [JsonProperty("userInfo")] public EventUserInfo UserInfo { get; set; }

        public IEvent GetEvent(IEventTypeResolver eventTypeResolver)
        {
            Type eventType = eventTypeResolver.GetEventType(EventType);

            return (IEvent)EventData.ToObject(eventType);
        }
    }
}