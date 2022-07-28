using System.Threading;
using System.Threading.Tasks;
using Domain.ES.EventStore;
using Domain.ES.Projections;
using Domain.Projections;
using Infrastructure.ES.Projections.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Port.BackgroundService.CosmosProjections
{
    public class CosmosProjectionsProcessingEngine : IScopedProcessingService
    {
        private readonly ILogger<CosmosProjectionsProcessingEngine> _logger;
        private readonly IConfiguration _configuration;

        public CosmosProjectionsProcessingEngine(
            IConfiguration configuration,
            ILogger<CosmosProjectionsProcessingEngine> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            string endpointUrl = _configuration["Azure:EventStore:Url"];
            string authorizationKey = _configuration["Azure:EventStore:AuthKey"];
            string databaseId = _configuration["Azure:EventStore:DatabaseId"];

            IEventTypeResolver eventTypeResolver = new EventTypeResolver();
            IViewRepository viewRepository = new CosmosViewRepository(endpointUrl, authorizationKey, databaseId);

            IProjectionEngine projectionEngine =
                new CosmosProjectionEngine(eventTypeResolver, viewRepository,
                    endpointUrl, authorizationKey, databaseId);
            projectionEngine.RegisterProjector(new AdvertiserProjector());
            projectionEngine.RegisterProjector(new CampaignProjector());

            await projectionEngine.StartAsync("TestInstance");

            await Task.Delay(-1); // keep it running
        }
    }
}