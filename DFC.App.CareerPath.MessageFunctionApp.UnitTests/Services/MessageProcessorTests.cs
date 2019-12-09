using DFC.App.CareerPath.Data.Enums;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.Services
{
    public class MessageProcessorTests
    {
        private const long SequenceNumber = 123;
        private const string BaseMessage = "Dummy Serialised Message";
        private const int InvalidEnumValue = 999;

        private readonly IMessageProcessor processor;
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;

        public MessageProcessorTests()
        {
            var expectedMappedModel = GetSegmentModel();

            mappingService = A.Fake<IMappingService>();
            A.CallTo(() => mappingService.MapToSegmentModel(A<string>.Ignored, A<long>.Ignored)).Returns(expectedMappedModel);

            httpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            processor = new MessageProcessor(httpClientService, mappingService);
        }

        [Fact]
        public async Task ProcessAsyncReturnsInternalServerErrorWhenInvalidMessageContentTypeSent()
        {
            // Act
            var result = await processor
                .ProcessAsync(BaseMessage, SequenceNumber, (MessageContentType)InvalidEnumValue, MessageActionType.Published)
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result);
        }

        [Fact]
        public async Task ProcessAsyncArgumentOutOfRangeExceptionWhenInvalidMessageActionSent()
        {
            await Assert.ThrowsAnyAsync<ArgumentOutOfRangeException>(async () => await processor.ProcessAsync(BaseMessage, SequenceNumber, MessageContentType.JobProfile, (MessageActionType)InvalidEnumValue).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ProcessAsyncCallsDeleteAsyncWhenDeletedMessageActionSent()
        {
            // Act
            var result = await processor
                .ProcessAsync(BaseMessage, SequenceNumber, MessageContentType.JobProfile, MessageActionType.Deleted)
                .ConfigureAwait(false);

            // Assert
            A.CallTo(() => httpClientService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ProcessAsyncCallsPutAsyncAndReturnsOkResultWhenDataExists()
        {
            // Arrange
            var putHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => putHttpClientService.PutAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.OK);

            var putMessageProcessor = new MessageProcessor(putHttpClientService, mappingService);

            // Act
            var result = await putMessageProcessor
                .ProcessAsync(BaseMessage, SequenceNumber, MessageContentType.JobProfile, MessageActionType.Published)
                .ConfigureAwait(false);

            // Assert
            A.CallTo(() => putHttpClientService.PutAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ProcessAsyncCallsPostAsyncAndReturnsOkResultWhenDataDoesntExist()
        {
            // Arrange
            var postHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => postHttpClientService.PutAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => postHttpClientService.PostAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.OK);

            var postMessageProcessor = new MessageProcessor(postHttpClientService, mappingService);

            // Act
            var result = await postMessageProcessor
                .ProcessAsync(BaseMessage, SequenceNumber, MessageContentType.JobProfile, MessageActionType.Published)
                .ConfigureAwait(false);

            // Assert
            A.CallTo(() => postHttpClientService.PutAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => postHttpClientService.PostAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(HttpStatusCode.OK, result);
        }

        private CareerPathSegmentModel GetSegmentModel()
        {
            return new CareerPathSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                SequenceNumber = 1,
                SocLevelTwo = "12",
                CanonicalName = "job-1",
                Data = new CareerPathSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    Markup = "<h1>Some Markup</h1>",
                },
            };
        }
    }
}