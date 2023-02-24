using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.ffff ");
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var logger = loggerFactory.CreateLogger<Program>();

            var connectionBuilder = new HubConnectionBuilder()
                .WithUrl("https://localhost:7135/hubs/streaming");
            connectionBuilder.Services.AddSingleton(_ => loggerFactory);

            var hubConnection = connectionBuilder.Build();
            await hubConnection.StartAsync();

            var cts = new CancellationTokenSource(3333);

            var stream = hubConnection.StreamAsync<int>("Counter", 10, 1000, cts.Token);

            try
            {
                await foreach (var item in stream)
                {
                    logger.LogInformation("Received item {item}", item);
                }

                logger.LogInformation("Streaming completed");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Streaming canceled");
            }

            Console.Read();
        }
    }
}
