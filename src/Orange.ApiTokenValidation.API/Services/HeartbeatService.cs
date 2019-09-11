using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.API.Services
{
    /// <summary>
    /// Heartbeat service
    /// </summary>
    public class HeartbeatService : IHostedService, IDisposable
    {
        private readonly IMeasurer _measurer;
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly TimeSpan _period;
        private CancellationTokenSource _cancellationTokenSource;

        public HeartbeatService(IMeasurer measurer, 
                                ILogger<HeartbeatService> logger)
        {
            _measurer = measurer;
            _logger = logger;
            _period = TimeSpan.FromSeconds(5);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async (state) => await HeartbeatAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token, _period, TimeSpan.Zero);

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;

            _timer?.Dispose();
            _timer = null;
        }

        private async Task HeartbeatAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Heartbeat");
            _measurer.Heartbeat();

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _timer?.Change(_period, TimeSpan.Zero);
        }
    }
}