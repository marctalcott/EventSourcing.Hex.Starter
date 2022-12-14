using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.ES.Events;
using Domain.ES.EventStore;
using Domain.ES.Exceptions;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.ES.EventStore
{
    public class CosmosDBEventStore : IEventStore
    {
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly CosmosClient _client;
        private readonly string _databaseId;
        private readonly string _containerId;

        public CosmosDBEventStore(
            IEventTypeResolver eventTypeResolver,
            string endpointUrl, string authorizationKey,
            string databaseId, string containerId)
        {
            _eventTypeResolver = eventTypeResolver;
            _client = new CosmosClient(endpointUrl, authorizationKey);
            _databaseId = databaseId;
            _containerId = containerId;
        }

        public async Task<IEventStream> LoadStreamAsyncOrThrowNotFound(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                               + " WHERE e.stream.id = @streamId"
                               + " ORDER BY e.stream.version";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;

                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }

            if (events.Count == 0)
            {
                throw new NotFoundException();
            }

            return new EventStream(streamId, version, events);
        }

        public async Task<IEventStream> LoadStreamAsync(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                               + " WHERE e.stream.id = @streamId"
                               + " ORDER BY e.stream.version";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;

                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }

            return new EventStream(streamId, version, events);
        }

        public async Task<EventWrapper> LoadEventWrapperAsync(string eventId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = "SELECT * FROM events e"
                               + " WHERE e.id = @eventId";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@eventId", eventId);

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);

            FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
            EventWrapper item = response.Resource.FirstOrDefault();
            return item;
        }

        public async Task<IEventStream> LoadStreamAsync(string streamId, int fromVersion)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                               + " WHERE e.stream.id = @streamId AND e.stream.version >= @fromVersion"
                               + " ORDER BY e.stream.version";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId)
                .WithParameter("@fromVersion", fromVersion);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;
                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }

            return new EventStream(streamId, version, events);
        }

        public async Task<bool> AppendToStreamAsync(
            EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            dynamic[] parameters = new dynamic[]
            {
                streamId,
                expectedVersion,
                SerializeEvents(eventUserInfo, streamId, expectedVersion, events)
            };

            return await container.Scripts.ExecuteStoredProcedureAsync<bool>("spAppendToStream", partitionKey,
                parameters);
        }

        private static string SerializeEvents(
            EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            if (string.IsNullOrEmpty(eventUserInfo.AuthServiceUserId))
                throw new Exception("UserInfo.Id must be set to a value.");

            var items = events.Select(e => new EventWrapper
            {
                Id = $"{streamId}:{++expectedVersion}",
                StreamInfo = new StreamInfo
                {
                    Id = streamId,
                    Version = expectedVersion
                },
                EventType = e.GetType().Name,
                EventData = JObject.FromObject(e),
                UserInfo = eventUserInfo
            });

            return JsonConvert.SerializeObject(items);
        }

        #region Snapshot Functionality

        private async Task<TSnapshot> LoadSnapshotAsync<TSnapshot>(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            var response = await container.ReadItemAsync<TSnapshot>(streamId, partitionKey);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Resource;
            }

            return default(TSnapshot);
        }

        #endregion
    }
}