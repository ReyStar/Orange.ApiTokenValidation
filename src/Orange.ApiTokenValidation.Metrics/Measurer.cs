using System;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Prometheus;
using Prometheus.DotNetRuntime;

namespace Orange.ApiTokenValidation.Metrics
{
    internal class Measurer: IMeasurer, IDisposable
    {
        private IDisposable _collector;
        private readonly Counter _heartbeatCounter;
        private readonly Counter _httpRequestCounter;
        private MetricServer _server;
        private MetricPusher _pusher;

        public Measurer(IOptions<MeasurerConfiguration> measurerConfigurationOptions, 
                        IOptions<CommonConfiguration> commonConfigurationOptions)
        {
            var measurerConfiguration = measurerConfigurationOptions.Value;

            _collector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

            _heartbeatCounter = Prometheus.Metrics.CreateCounter("heartbeat_count", "heartbeat");

            _httpRequestCounter = Prometheus.Metrics.CreateCounter("request_total", "HTTP Requests Total",
                new CounterConfiguration
                {
                    LabelNames = new[] {"path", "method", "status", "correlation-id" }
                });

            if (measurerConfiguration.UsePushMetricsServer)
            {
                //push model
                _pusher = new MetricPusher(new MetricPusherOptions
                {
                    Endpoint = measurerConfiguration.PushMetricsServerEndpoint,
                    Job = "push-metrics-job",
                    Instance = commonConfigurationOptions.Value.InstanceId,
                    IntervalMilliseconds = (long) measurerConfiguration.FlushPeriod.TotalMilliseconds
                });

                _pusher.Start();
            }
        }

        public void Heartbeat()
        {
            _heartbeatCounter.Inc();
        }

        public void RequestMetric(string path, string method, int statusCode, string correlationId)
        {
            _httpRequestCounter.Labels(path, method, statusCode.ToString(), correlationId).Inc();
        }

        public void Dispose()
        {
            Push();

            var collector = _collector;
            if (collector != null)
            {
                _collector = null;
                collector.Dispose();
            }

            var server = _server;
            if (server != null)
            {
                _server = null;
                server.Stop();
            }

            var pusher = _pusher;
            if(pusher!=null)
            {
                _pusher = null;
                pusher.Stop();
            }
        }

        private void Push()
        {
            _heartbeatCounter.Publish();
            _httpRequestCounter.Publish();
        }
    }
}
