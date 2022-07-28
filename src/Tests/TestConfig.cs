namespace Tests
{
    public class TestConfig
    {
        public string EndpointUrl { get; init; } = "";
        public string AuthorizationKey { get; init; } = "";
        public string DatabaseId { get; init; } = "";
        public string EventsContainer { get; init; } = "";
        public string ViewsContainer { get; init; } = "";
        public string LeasesContainer { get; init; } = "";

        public TestConfig()
        {
            
        }
    }
}