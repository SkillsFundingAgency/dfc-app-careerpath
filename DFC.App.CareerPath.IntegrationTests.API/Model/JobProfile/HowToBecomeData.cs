using System;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.JobProfile
{
    public class HowToBecomeData
    {
        public List<RouteEntry> RouteEntries { get; set; }

        public FurtherInformationContentType FurtherInformation { get; set; }

        public FurtherRoutes FurtherRoutes { get; set; }

        public string IntroText { get; set; }

        public List<Registration> Registrations { get; set; }
    }
}
