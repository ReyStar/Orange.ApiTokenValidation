using System;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Prometheus;
using Prometheus.DotNetRuntime;

//using Prometheus.DotNetRuntime;

namespace Orange.ApiTokenValidation.Metrics
{
    internal class Measurer: IMeasurer, IDisposable
    {
        private IDisposable _collector;
        private readonly Counter _heartbeatCounter;
        private readonly Counter _httpRequestCounter;

        public Measurer()
        {
            _collector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

            _heartbeatCounter = Prometheus.Metrics.CreateCounter("heartbeat_count", "Application Heartbeat activity");

            _httpRequestCounter = Prometheus.Metrics.CreateCounter("request_total", "HTTP Requests Total",
                new CounterConfiguration
                {
                    LabelNames = new[] {"path", "method", "status"}
                });
        }

        public void Heartbeat()
        {
            _heartbeatCounter.Inc();
        }

        public void RequestMetric(string path, string method, int statusCode)
        {
            _httpRequestCounter.Labels(path, method, statusCode.ToString()).Inc();
        }

        public void Push()
        {
            _heartbeatCounter.Publish();
            _httpRequestCounter.Publish();
        }

        public void Dispose()
        {
            Push();
            
            _collector?.Dispose();
            _collector = null;
        }
    }
}
