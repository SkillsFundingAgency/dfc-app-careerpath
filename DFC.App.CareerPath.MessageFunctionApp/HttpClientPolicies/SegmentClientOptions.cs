using System;

namespace DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies
{
    public class SegmentClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 10);
    }
}