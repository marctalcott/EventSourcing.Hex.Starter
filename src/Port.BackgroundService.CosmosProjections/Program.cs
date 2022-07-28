using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Port.BackgroundService.CosmosProjections;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<IScopedProcessingService, CosmosProjectionsProcessingEngine>();
    })
    .Build();

await host.RunAsync();