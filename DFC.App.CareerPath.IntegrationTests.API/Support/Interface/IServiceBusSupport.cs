using DFC.App.CareerPath.Tests.Common.AzureServiceBusSupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Enums;
using System;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IServiceBusSupport
    {
        Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ActionType actionType, CType ctype);

        Message CreateServiceBusMessage(string messageId, byte[] messageBody, ActionType actionType, CType ctype);

        Task SendMessage(Topic topic, Message message);
    }
}
