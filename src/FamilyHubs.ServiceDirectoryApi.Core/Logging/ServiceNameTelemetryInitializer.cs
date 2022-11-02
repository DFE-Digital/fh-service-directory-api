﻿using fh_service_directory_api.core.Utility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

    public class ServiceNameTelemetryInitializer : ITelemetryInitializer
    {
        private string _serviceName;

        public ServiceNameTelemetryInitializer(string serviceName)
        {
            Guard.IsNullOrWhiteSpace(serviceName, nameof(serviceName));
            _serviceName = serviceName;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (!telemetry.Context.GlobalProperties.ContainsKey(LoggingConstants.ServiceNamePropertiesName))
            {
                telemetry.Context.GlobalProperties.Add(LoggingConstants.ServiceNamePropertiesName, _serviceName);
            }
        }
    }
