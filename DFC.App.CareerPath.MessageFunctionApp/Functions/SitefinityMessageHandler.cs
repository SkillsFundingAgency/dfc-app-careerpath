using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Common.Services;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Functions
{
    public class SitefinityMessageHandler
    {
        private readonly IMessagePreProcessor messagePreProcessor;
        private readonly ILogService logService;
        private readonly ICorrelationIdProvider correlationIdProvider;

        public SitefinityMessageHandler(IMessagePreProcessor messagePreProcessor, ILogService logService, ICorrelationIdProvider correlationIdProvider)
        {
            this.messagePreProcessor = messagePreProcessor;
            this.logService = logService;
            this.correlationIdProvider = correlationIdProvider;
        }

        [FunctionName("SitefinityMessageHandler")]
        public async Task Run([ServiceBusTrigger("%cms-messages-topic%", "%cms-messages-subscription%", Connection = "service-bus-connection-string")] Message sitefinityMessage)
        {
            if (sitefinityMessage == null)
            {
                logService.LogInformation("Received null message");
            }

            correlationIdProvider.CorrelationId = sitefinityMessage.CorrelationId;

            logService.LogInformation("Received message");
            await messagePreProcessor.Process(sitefinityMessage).ConfigureAwait(false);
            logService.LogInformation("Processed message");
        }
    }
}
