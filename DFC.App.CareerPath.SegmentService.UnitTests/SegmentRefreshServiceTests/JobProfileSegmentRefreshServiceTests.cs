using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
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

        [Fact]
        public async Task SendMessageListDoesNotSendMessagesWhenListIsNull()
        {
            // Arrange
            var fakeTopicClient = A.Fake<ITopicClient>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var refreshService = new JobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>(fakeTopicClient, correlationIdProvider);

            // Act
            await refreshService.SendMessageListAsync(null).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeTopicClient.SendAsync(A<Message>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task SendMessageListSendsListOfMessagesOnTopicClient()
        {
            // Arrange
            var fakeTopicClient = A.Fake<ITopicClient>();
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var refreshService = new JobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>(fakeTopicClient, correlationIdProvider);

            var model = new List<RefreshJobProfileSegmentServiceBusModel>
            {
                new RefreshJobProfileSegmentServiceBusModel
                {
                    CanonicalName = "some-canonical-name-1",
                    JobProfileId = Guid.NewGuid(),
                    Segment = "CareerPathsAndProgression",
                },
                new RefreshJobProfileSegmentServiceBusModel
                {
                    CanonicalName = "some-canonical-name-2",
                    JobProfileId = Guid.NewGuid(),
                    Segment = "CareerPathsAndProgression",
                },
                new RefreshJobProfileSegmentServiceBusModel
                {
                    CanonicalName = "some-canonical-name-3",
                    JobProfileId = Guid.NewGuid(),
                    Segment = "CareerPathsAndProgression",
                },
            };

            // Act
            await refreshService.SendMessageListAsync(model).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeTopicClient.SendAsync(A<List<Message>>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}