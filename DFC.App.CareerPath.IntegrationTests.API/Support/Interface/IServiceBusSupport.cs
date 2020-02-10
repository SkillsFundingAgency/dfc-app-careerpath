using DFC.App.CareerPath.Tests.Common.AzureServiceBusSupport;
using System;
using System.Threading.Tasks;
using static DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.EnumLibrary;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IServiceBusSupport
    {
        Message CreateServiceBusMessage(Guid messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);

        Message CreateServiceBusMessage(string messageId, byte[] messageBody, ContentType contentType, ActionType actionType, CType ctype);

        Task SendMessage(Topic topic, Message message);
    }
}
