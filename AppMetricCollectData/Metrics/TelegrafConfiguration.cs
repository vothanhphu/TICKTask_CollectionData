using App.Metrics;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Json;
using System;

namespace AppMetricCollectData.Metrics
{
    public static class TelegrafConfiguration
    {
        public static IMetricsRoot ConfigMetrics()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Timer);
            var builder = new MetricsBuilder().Report.OverHttp(options =>
                {
                    options.HttpSettings.RequestUri = new Uri("http://172.17.0.2/metrics");
                    options.HttpSettings.UserName = "telegrauser";
                    options.HttpSettings.Password = "Password";
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                    options.Filter = filter;
                    options.FlushInterval = TimeSpan.FromSeconds(20);
                });

            builder.Configuration.Configure(option =>
                {
                    option.DefaultContextLabel = "ContextLevel";
                    option.Enabled = true;
                    option.ReportingEnabled = true;
                    option.GlobalTags.Add("MyKey", "NyValue");
                });

            return builder.Build();
        }
    }
}
