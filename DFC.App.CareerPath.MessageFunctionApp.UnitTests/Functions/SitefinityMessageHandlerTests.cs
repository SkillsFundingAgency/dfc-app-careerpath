using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Common.Services;
using DFC.App.CareerPath.Data.Enums;
using DFC.App.CareerPath.MessageFunctionApp.Functions;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using FakeItEasy;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.Functions
{
    public class SitefinityMessageHandlerTests
    {
        private readonly IMessagePreProcessor defaultProcessor;
        private readonly SitefinityMessageHandler messageHandler;

        public SitefinityMessageHandlerTests()
        {
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            var telemetryClient = new TelemetryClient();
            var logService = new LogService(correlationIdProvider, telemetryClient);
            defaultProcessor = A.Fake<IMessagePreProcessor>();

            messageHandler = new SitefinityMessageHandler(defaultProcessor, logService, correlationIdProvider);
        }

        [Fact]
        public async Task RunHandlerThrowsArgumentNullExceptionWhenMessageIsNull()
        {
            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await messageHandler.Run(null).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task RunHandlerCallsMessagePreProcessor()
        {
            // Arrange
            var message = CreateBaseMessage();

            // Act
            await messageHandler.Run(message).ConfigureAwait(false);

            // Assert
            A.CallTo(() => defaultProcessor.Process(A<Message>.Ignored)).MustHaveHappenedOnceExactly();
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