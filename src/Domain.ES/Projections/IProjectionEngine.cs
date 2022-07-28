using System.Threading.Tasks;

namespace Domain.ES.Projections
{
    public interface IProjectionEngine
    {
        void RegisterProjector(IProjector projector);

        Task StartAsync(string instanceName);

        Task StopAsync();
    }
}