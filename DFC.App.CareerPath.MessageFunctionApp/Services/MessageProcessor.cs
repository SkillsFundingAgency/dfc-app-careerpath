using DFC.App.CareerPath.Data.Enums;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;
        private readonly ILogService logService;

        public MessageProcessor(IHttpClientService httpClientService, IMappingService mappingService, ILogService logService)
        {
            this.httpClientService = httpClientService;
            this.mappingService = mappingService;
            this.logService = logService;
        }

        public async Task<HttpStatusCode> ProcessAsync(string message, long sequenceNumber, MessageContentType messageContentType, MessageActionType messageAction)
        {
            logService.LogInformation($"{nameof(ProcessAsync)} has been called");

            switch (messageContentType)
            {
                case MessageContentType.JobProfile:
                    return await ProcessJobProfileMessageAsync(message, messageAction, sequenceNumber).ConfigureAwait(false);

                default:
                    break;
            }

            return await Task.FromResult(HttpStatusCode.InternalServerError).ConfigureAwait(false);
        }

        private async Task<HttpStatusCode> ProcessJobProfileMessageAsync(string message, MessageActionType messageAction, long sequenceNumber)
        {
            logService.LogInformation($"{nameof(ProcessJobProfileMessageAsync)} has been called");

            var jobProfile = mappingService.MapToSegmentModel(message, sequenceNumber);

            switch (messageAction)
            {
                case MessageActionType.Draft:
                case MessageActionType.Published:
                    var result = await httpClientService.PutAsync(jobProfile).ConfigureAwait(false);
                    if (result == HttpStatusCode.NotFound)
                    {
                        logService.LogInformation($"Sending a PUT request with job profile {nameof(jobProfile)} returned 404 Not Found");

                        return await httpClientService.PostAsync(jobProfile).ConfigureAwait(false);
                    }

                    return result;

                case MessageActionType.Deleted:
                    return await httpClientService.DeleteAsync(jobProfile.DocumentId).ConfigureAwait(false);

                default:
                    throw new ArgumentOutOfRangeException(nameof(messageAction), $"Invalid message action '{messageAction}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageActionType)))}'");
            }
        }
    }
}