using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class StreamingHub : Hub
    {
        private readonly ILogger<StreamingHub> _logger;

        public StreamingHub(ILogger<StreamingHub> logger)
        {
            _logger = logger;
        }

        public async IAsyncEnumerable<int> Counter(
            int count,
            int delay,
            [EnumeratorCancellation]
            CancellationToken cancellationToken)
        {
            for (var i = 0; i < count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("yield {Count}", i);
                yield return i;

                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
