using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Orange.ApiTokenValidation.API.Services
{
    /// <summary>
    /// Heartbeat service
    /// </summary>
    public class HeartbeatService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly TimeSpan _period;

        public HeartbeatService(ILogger<HeartbeatService> logger)
        {
            _logger = logger;
            _period = TimeSpan.FromSeconds(5);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async (state) => await HeartbeatAsync((CancellationToken) state), cancellationToken, _period, TimeSpan.Zero);

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private async Task HeartbeatAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Heartbeat");

            _timer?.Change(_period, TimeSpan.Zero);
        }
    }
}