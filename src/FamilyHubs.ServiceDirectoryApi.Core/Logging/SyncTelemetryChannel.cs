using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

    public class SyncTelemetryChannel : ITelemetryChannel
    {
        private Uri _endpoint;

        public SyncTelemetryChannel(string endpointUrl)
        {
                _endpoint = new Uri(endpointUrl);
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; } = default!;

    public void Send(ITelemetry item)
        {
            byte[] json = JsonSerializer.Serialize(new List<ITelemetry>() { item }, true);

            Transmission transimission = new Transmission(_endpoint, json, "application/x-json-stream", JsonSerializer.CompressionType);

            Task<HttpWebResponseWrapper> sendTask = transimission.SendAsync();

            sendTask.Wait();
        }

        public void Flush() { }

        public void Dispose() { }

    }
