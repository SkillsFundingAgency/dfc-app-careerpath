using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies
{
    public class SegmentClientOptions
    {
        public Uri BaseAddress { get; set; }

        public string GetEndpoint { get; set; }
        public string PatchEndpoint { get; set; }
        public string PostEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);         // default to 30 seconds
    }
}
