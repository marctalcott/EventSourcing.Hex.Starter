using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.ES.SnapshotStore
{
    public class Snapshot
    {
        [JsonProperty("id")] public string StreamId { get; set; }

        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("snapshotData")] public JObject SnapshotData { get; set; }
    }
}