using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentRefreshServiceTests
{
    [Trait("Segment Refresh Service", "Send Message Tests")]
    public class JobProfileSegmentRefreshServiceTests
    {
        [Fact]
        public async Task JobProfileSegmentRefreshServiceTestsSendMessage()
        {
            // Arrange
            var fakeTopicClient = A.Fake<ITopicClient>();
            var fakeCorrelationIdProvider = A.Fake<ICorrelationIdProvider>();
            var refreshService = new JobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>(fakeTopicClient, fakeCorrelationIdProvider);

            var model = new RefreshJobProfileSegmentServiceBusModel
            {
                CanonicalName = "some-canonical-name-1",
                JobProfileId = Guid.NewGuid(),
                Segment = "CareerPathsAndProgression",
            };

            // Act
            await refreshService.SendMessageAsync(model).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeTopicClient.SendAsync(A<Message>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}