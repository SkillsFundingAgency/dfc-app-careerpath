using Microsoft.Azure.ServiceBus;

namespace DFC.App.CareerPath.Tests.IntegrationTests.API.Support.AzureServiceBus.ServiceBusFactory.Interface
{
    public interface IMessageFactory
    {
        Message Create(string messageId, byte[] messageBody, string actionType, string contentType);
    }
}
