using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using FakeItEasy;
using System.Net;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentRefreshServiceTests
{
    [Trait("Segment Refresh Service", "Send Message Tests")]
    public class JobProfileSegmentRefreshServiceTests
    {
        private readonly ServiceBusOptions serviceBusOptions;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegment> jobProfileSegmentRefreshService;

        public JobProfileSegmentRefreshServiceTests()
        {
            serviceBusOptions = A.Fake<ServiceBusOptions>();
            jobProfileSegmentRefreshService = new JobProfileSegmentRefreshService<RefreshJobProfileSegment>(serviceBusOptions);
        }

        [Fact]
        public void JobProfileSegmentRefreshServiceTestsSendMessage()
        {
            // arrange
            var refreshJobProfileSegment = A.Fake<RefreshJobProfileSegment>();

            // act
            jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegment);

            // assert
        }
    }
}
