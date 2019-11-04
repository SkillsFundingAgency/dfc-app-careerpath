using DFC.App.CareerPath.Data.Constants;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class LogService : ILogService
    {
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly TelemetryClient telemetryClient;

        public LogService(ICorrelationIdProvider correlationIdProvider, TelemetryClient telemetryClient)
        {
            this.correlationIdProvider = correlationIdProvider;
            this.telemetryClient = telemetryClient;
        }

        public void LogInformation(string message)
        {
            Log(message, SeverityLevel.Information);
        }

        private void Log(string message, SeverityLevel severityLevel)
        {
            var properties = new Dictionary<string, string>();
            properties.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
            telemetryClient.TrackTrace(message, severityLevel, properties);
        }
    }
}
