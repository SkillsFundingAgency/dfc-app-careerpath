using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Data.Enums;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using FakeItEasy;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.Services
{
    public class MessagePreProcessorTests
    {
        private readonly IMessagePreProcessor defaultMessagePreProcessor;

        public MessagePreProcessorTests()
        {
            var defaultProcessor = A.Fake<IMessageProcessor>();
            var defaultMessagePropertiesService = A.Fake<IMessagePropertiesService>();
            var defaultLogService = A.Fake<ILogService>();

            defaultMessagePreProcessor = new MessagePreProcessor(defaultProcessor, defaultLogService, defaultMessagePropertiesService);
        }

        public static IEnumerable<object[]> ValidStatusCodes => new List<object[]>
        {
            new object[] { HttpStatusCode.OK },
            new object[] { HttpStatusCode.Created },
            new object[] { HttpStatusCode.AlreadyReported },
        };

        [Fact]
        public async Task RunHandlerThrowsArgumentNullExceptionWhenMessageIsNull()
        {
            // Arrange

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await defaultMessagePreProcessor.Process((Message)null).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentExceptionWhenMessageBodyIsEmpty()
        {
            // Arrange
            var message = new Message(Encoding.ASCII.GetBytes(string.Empty));

            // Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await defaultMessagePreProcessor.Process(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentOutOfRangeExceptionWhenMessageActionIsInvalid()
        {
            // Arrange
            var message = CreateBaseMessage(messageAction: (MessageActionType)999);

            // Act
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await defaultMessagePreProcessor.Process(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentOutOfRangeExceptionWhenMessageContentTypeIsInvalid()
        {
            // Arrange
            var message = CreateBaseMessage(contentType: (MessageContentType)999);

            // Act
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await defaultMessagePreProcessor.Process(message).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerLogsWarningWhenMessageProcessorReturnsError()
        {
            // Arrange
            var message = CreateBaseMessage();
            var logger = A.Fake<ILogService>();

            var processor = A.Fake<IMessageProcessor>();
            A.CallTo(() => processor.ProcessAsync(A<string>.Ignored, A<long>.Ignored, A<MessageContentType>.Ignored, A<MessageActionType>.Ignored)).Returns(HttpStatusCode.InternalServerError);

            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            A.CallTo(() => messagePropertiesService.GetSequenceNumber(A<Message>.Ignored)).Returns(123);

            var messagePreProcessor = new MessagePreProcessor(processor, logger, messagePropertiesService);

            // Act
            await messagePreProcessor.Process(message).ConfigureAwait(false);

            // Assert
            A.CallTo(() => logger.LogWarning(A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [MemberData(nameof(ValidStatusCodes))]
        public async Task RunHandlerLogsInformationWhenMessageProcessorReturnsSuccess(HttpStatusCode status)
        {
            // Arrange
            var message = CreateBaseMessage();
            var logger = A.Fake<ILogService>();

            var processor = A.Fake<IMessageProcessor>();
            A.CallTo(() => processor.ProcessAsync(A<string>.Ignored, A<long>.Ignored, A<MessageContentType>.Ignored, A<MessageActionType>.Ignored)).Returns(status);

            var messagePropertiesService = A.Fake<IMessagePropertiesService>();
            A.CallTo(() => messagePropertiesService.GetSequenceNumber(A<Message>.Ignored)).Returns(123);

            // Act
            var messagePreProcessor = new MessagePreProcessor(processor, logger, messagePropertiesService);
            await messagePreProcessor.Process(message).ConfigureAwait(false);

            // Assert
            A.CallTo(() => logger.LogInformation(A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        private Message CreateBaseMessage(MessageActionType messageAction = MessageActionType.Published, MessageContentType contentType = MessageContentType.JobProfile)
        {
            var message = A.Fake<Message>();
            message.Body = Encoding.ASCII.GetBytes("Some body json object here");
            message.UserProperties.Add("ActionType", messageAction.ToString());
            message.UserProperties.Add("CType", contentType);

            return message;
        }
    }
}