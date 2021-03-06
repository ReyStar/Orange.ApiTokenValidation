﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orange.ApiTokenValidation.Application.Interfaces;

namespace Orange.ApiTokenValidation.Shell
{
    class HeartbeatService: BackgroundService
    {
        private readonly TimeSpan _period;
        private readonly IMeasurer _measurer;
        private readonly ILogger _logger;

        public HeartbeatService(IMeasurer measurer, ILogger<HeartbeatService> logger)
        {
            _measurer = measurer;
            _logger = logger;
            _period = TimeSpan.FromSeconds(5);
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("Heartbeat");
                    _measurer.Heartbeat();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Send Heartbeat error");
                }

                await Task.Delay(_period, token);
            }
        }
    }
}
