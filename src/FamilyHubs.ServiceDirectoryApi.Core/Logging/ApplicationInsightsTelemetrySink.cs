using fh_service_directory_api.core.Interfaces.Logging;
using fh_service_directory_api.core.Utility;
using Microsoft.ApplicationInsights;

    public class ApplicationInsightsTelemetrySink : ITelemetry
    {
        private readonly TelemetryClient _client;

        public ApplicationInsightsTelemetrySink(TelemetryClient client)
        {
            Guard.ArgumentNotNull(client, nameof(client));
            _client = client;
        }

        public void TrackEvent(string eventName, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null)
        {
            _client.TrackEvent(eventName, properties, metrics);
        }
    }
