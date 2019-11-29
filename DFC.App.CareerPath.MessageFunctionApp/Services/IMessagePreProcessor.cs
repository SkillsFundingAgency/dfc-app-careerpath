using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IMessagePreProcessor
    {
        Task Process(Message sitefinityMessage);
    }
}