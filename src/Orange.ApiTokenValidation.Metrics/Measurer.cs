using System;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace Orange.ApiTokenValidation.Metrics
{
    internal class Measurer: IMeasurer, IDisposable
    {
        private IDisposable _collector;
        private readonly Counter _heartbeatCount;

        public Measurer()
        {
            _collector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

            _heartbeatCount = Prometheus.Metrics.CreateCounter("heartbeat_count", "Application Heartbeat activity");
        }

        public void Heartbeat()
        {
            _heartbeatCount.Inc();
        }

        public void Dispose()
        {
            _heartbeatCount.Publish();

            _collector?.Dispose();
            _collector = null;
        }
    }
}
