using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

    public class SyncTelemetryChannel : ITelemetryChannel
    {
        private Uri _endpoint;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SyncTelemetryChannel(string endpointUrl)
    {
            _endpoint = new Uri(endpointUrl);
    }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

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
