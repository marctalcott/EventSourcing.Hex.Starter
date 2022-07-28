using Infrastructure.ES.EventStore;
using Newtonsoft.Json;

namespace Infrastructure.ES.Projections.Common
{
    public class Change : EventWrapper
    {
        [JsonProperty("_lsn")] public long LogicalSequenceNumber { get; set; }
    }
}