using System;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    public static class Settings
    {
        public static TimeSpan GracePeriod { get; set; }

        public static class ServiceBusConfig
        {
            public static string ConnectionString { get; set; }
        }

        public static class APIConfig
        {
            public static string Version { get; set; }

            public static string ApimSubscriptionKey { get; set; }

            public static string EndpointBaseUrl { get; set; }
        }
    }
}
