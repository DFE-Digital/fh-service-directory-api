using fh_service_directory_api.core.Utility;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace fh_service_directory_api.core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationInsightsTelemetryClient(this IServiceCollection builder, IConfiguration config, string serviceName, TelemetryChannelType channelType = TelemetryChannelType.Default)
        {
            Guard.ArgumentNotNull(config, nameof(config));


            string appInsightsKey = config["APPINSIGHTS_INSTRUMENTATIONKEY"];
            string appInsightsConnectionString = config["APPINSIGHTS_CONNECTION_STRING"];
            string appInsightsURL = "";
            if (appInsightsConnectionString.Split('=').Length == 2)
            {
                appInsightsURL = appInsightsConnectionString.Split('=')[2].Trim('/');
            }


            if (string.IsNullOrWhiteSpace(appInsightsKey))
            {
                throw new InvalidOperationException("Unable to lookup Application Insights Configuration key from Configuration Provider. The value returned was empty string");
            }

            builder.Configure((ApplicationInsightsServiceOptions options) =>
            {
#pragma warning disable CS0618 // Type or member is obsolete
                options.InstrumentationKey = appInsightsKey;
#pragma warning restore CS0618
                options.ConnectionString = appInsightsConnectionString;
                
            });

            if (channelType == TelemetryChannelType.Sync)
            {
                builder.AddSingleton(typeof(ITelemetryChannel), new SyncTelemetryChannel(appInsightsURL));
            }

            //builder.AddApplicationInsightsTelemetry(config);

          
            builder.AddScoped((ctx) =>
            {
                TelemetryConfiguration telemetryConfiguration = ctx.GetService<TelemetryConfiguration>() ?? new TelemetryConfiguration();   
                TelemetryClient client = new TelemetryClient(telemetryConfiguration)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    InstrumentationKey = appInsightsKey
#pragma warning restore CS0618
                };

                if (!client.Context.GlobalProperties.ContainsKey(LoggingConstants.ServiceNamePropertiesName))
                {
                    client.Context.GlobalProperties.Add(LoggingConstants.ServiceNamePropertiesName, serviceName);
                }

                return client;
            });

            return builder;
        }
        public static IServiceCollection AddApplicationInsightsServiceName(this IServiceCollection builder, IConfiguration config, string serviceName)
        {
            Guard.ArgumentNotNull(config, nameof(config));
            Guard.IsNullOrWhiteSpace(serviceName, nameof(serviceName));

            ServiceNameTelemetryInitializer serviceNameEnricher = new ServiceNameTelemetryInitializer(serviceName);

            builder.AddSingleton<ITelemetryInitializer>(serviceNameEnricher);

            return builder;
        }

        public static IServiceCollection AddLogging(this IServiceCollection builder, string serviceName, IConfigurationRoot? config = null)
        {
            builder.AddSingleton<ILogger>((ctx) =>
            {
                //TelemetryClient client = ctx.GetService<TelemetryClient>();            

                LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(serviceName);

                if (config != null && !string.IsNullOrWhiteSpace(config.GetValue<string>("FileLoggingPath")))
                {
                    string folderPath = config.GetValue<string>("FileLoggingPath");

                    loggerConfiguration.WriteTo.File(folderPath + "log-{Date}-" + Environment.MachineName + ".txt", LogEventLevel.Verbose);
                }

#if DEBUG
                loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
#endif

                return loggerConfiguration.CreateLogger();
            });

            return builder;
        }
        public static LoggerConfiguration GetLoggerConfiguration(string serviceName)
        {
            Guard.IsNullOrWhiteSpace(serviceName, nameof(serviceName));

            return new LoggerConfiguration()
            .Enrich.With(new ILogEventEnricher[]
            {
                new ServiceNameLogEnricher(serviceName)
            })
            .Enrich.FromLogContext()
            .WriteTo.ApplicationInsights(TelemetryConverter.Traces);
        }

        public static IServiceCollection AddTelemetry(this IServiceCollection builder)
        {

            builder.AddScoped<Interfaces.Logging.ITelemetry, ApplicationInsightsTelemetrySink>();


            return builder;
        }
    }
}
