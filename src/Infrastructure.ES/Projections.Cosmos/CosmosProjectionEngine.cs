using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.ES.EventStore;
using Domain.ES.Projections;
using Infrastructure.ES.Projections.Common;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.ES.Projections.Cosmos
{
    public class CosmosProjectionEngine : IProjectionEngine
    {
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly IViewRepository _viewRepository;
        private readonly string _endpointUrl;
        private readonly string _authorizationKey;
        private readonly string _databaseId;
        private readonly string _eventContainerId;
        private readonly string _leaseContainerId;
        private readonly List<IProjector> _projectors;
        private ChangeFeedProcessor _changeFeedProcessor;

        public CosmosProjectionEngine(
            IEventTypeResolver eventTypeResolver, IViewRepository viewRepository,
            string endpointUrl, string authorizationKey, string databaseId, string eventContainerId = "events",
            string leaseContainerId = "leases")
        {
            _eventTypeResolver = eventTypeResolver;
            _viewRepository = viewRepository;
            _endpointUrl = endpointUrl;
            _authorizationKey = authorizationKey;
            _databaseId = databaseId;
            _eventContainerId = eventContainerId;
            _leaseContainerId = leaseContainerId;
            _projectors = new List<IProjector>();
        }

        public void RegisterProjector(IProjector projector)
        {
            _projectors.Add(projector);
        }

        public Task StartAsync(string instanceName)
        {
            CosmosClient client = new CosmosClient(_endpointUrl, _authorizationKey);

            Container eventContainer = client.GetContainer(_databaseId, _eventContainerId);
            Container leaseContainer = client.GetContainer(_databaseId, _leaseContainerId);

            _changeFeedProcessor = eventContainer
                .GetChangeFeedProcessorBuilder<Change>("CosmosDBProjections", HandleChangesAsync)
                .WithInstanceName(instanceName)
                .WithLeaseContainer(leaseContainer)
                .WithStartTime(new DateTime(2020, 5, 1, 0, 0, 0, DateTimeKind.Utc))
                .Build();

            return _changeFeedProcessor.StartAsync();
        }

        public Task StopAsync()
        {
            return _changeFeedProcessor.StopAsync();
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<Change> changes, CancellationToken cancellationToken)
        {
            foreach (var change in changes)
            {
                var @event = change.GetEvent(_eventTypeResolver);

                var subscribedProjections = _projectors
                    .Where(projection => projection.IsSubscribedTo(@event));

                foreach (var projection in subscribedProjections)
                {
                    var viewName = projection.GetViewName(change.StreamInfo.Id, @event);

                    var handled = false;
                    while (!handled)
                    {
                        var view = (CosmosView)await _viewRepository.LoadViewAsync(viewName);

                        // Only update if the LSN of the change is higher than the view. This will ensure
                        // that changes are only processed once.
                        // NOTE: This only works if there's just a single physical partition in Cosmos DB.
                        // TODO: To support multiple partitions we need access to the leases to store
                        // a LSN per lease in the view. This is not yet possible in the V3 SDK.
                        if (view.IsNewerThanCheckpoint(change))
                        {
                            projection.Apply(@event, view);

                            view.UpdateCheckpoint(change);

                            handled = await _viewRepository.SaveViewAsync(viewName, view);
                        }
                        else
                        {
                            // Already handled.
                            handled = true;
                        }

                        if (!handled)
                        {
                            // Oh noos! Somebody changed the view in the meantime, let's wait and try again.
                            await Task.Delay(100);
                        }
                    }
                }
            }
        }
    }
}