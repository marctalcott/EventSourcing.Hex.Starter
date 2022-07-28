using System.Threading;
using System.Threading.Tasks;

namespace Port.BackgroundService.CosmosProjections
{
    public interface IScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}