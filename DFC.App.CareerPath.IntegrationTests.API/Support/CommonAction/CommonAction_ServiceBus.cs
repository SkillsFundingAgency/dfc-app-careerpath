using DFC.App.CareerPath.Tests.Common.AzureServiceBusSupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Enums;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface;
using System;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    internal partial class CommonAction : IServiceBusSupport
    {
        public Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ActionType actionType, CType ctype)
        {
            return this.CreateServiceBusMessage(messageId.ToString(), messageBody, actionType, ctype);
        }

        public Message CreateServiceBusMessage(string messageId, byte[] messageBody, ActionType actionType, CType ctype)
        {
            Message message = new Message();
            message.ContentType = "application/json";
            message.Body = messageBody;
            message.CorrelationId = Guid.NewGuid().ToString();
            message.Label = "Automated message";
            message.MessageId = messageId;
            message.UserProperties.Add("Id", messageId);
            message.UserProperties.Add("ActionType", actionType.ToString());
            message.UserProperties.Add("CType", ctype.ToString());
            return message;
        }

        public async Task SendMessage(Topic topic, Message message)
        {
            await topic.SendAsync(message).ConfigureAwait(true);
        }
    }
}
