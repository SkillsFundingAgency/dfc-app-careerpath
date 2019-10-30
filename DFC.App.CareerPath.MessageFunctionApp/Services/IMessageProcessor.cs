using DFC.App.CareerPath.Data.Models.ServiceBusModels.Save;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IMessageProcessor
    {
        Task<HttpStatusCode> Delete(Guid jobProfileId);

        Task<HttpStatusCode> Save(JobProfileServiceBusSaveModel message);
    }
}
