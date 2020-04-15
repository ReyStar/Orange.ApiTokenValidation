using System;

namespace Orange.ApiTokenValidation.Metrics
{
    class MeasurerConfiguration
    {
        public string PullMetricsUrl { get; set; } = "metrics/";
        public bool UsePushMetricsServer { get; set; } = true;
        public string PushMetricsServerEndpoint { get; set; }
        public TimeSpan FlushPeriod { get; set; } = TimeSpan.FromMilliseconds(1000);
    }
}
