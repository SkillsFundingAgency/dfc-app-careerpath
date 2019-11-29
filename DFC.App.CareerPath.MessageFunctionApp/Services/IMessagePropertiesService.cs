using Microsoft.Azure.ServiceBus;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IMessagePropertiesService
    {
        long GetSequenceNumber(Message message);
    }
}