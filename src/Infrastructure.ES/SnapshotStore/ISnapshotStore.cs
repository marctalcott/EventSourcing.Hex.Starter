using System.Threading.Tasks;

namespace Infrastructure.ES.SnapshotStore
{
    public interface ISnapshotStore
    {
        Task<Snapshot> LoadSnapshotAsync(string streamId);

        Task SaveSnapshotAsync(string streamId, int version, object snapshot);
    }
}