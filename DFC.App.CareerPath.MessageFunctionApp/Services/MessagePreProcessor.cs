using AutoMapper;
using DFC.App.CareerPath.Data.Constants;
using DFC.App.CareerPath.Data.Enums;
using DFC.App.CareerPath.Data.Models.ServiceBusModels.Save;
using DFC.App.CareerPath.Data.Models.ServiceBusModels.Save.Delete;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class MessagePreProcessor : IMessagePreProcessor
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly ILogger<MessagePreProcessor> logger;
        private readonly IMapper mapper;

        public MessagePreProcessor(IMessageProcessor messageProcessor, ILogger<MessagePreProcessor> logger, IMapper mapper)
        {
            this.messageProcessor = messageProcessor;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task Process(Message sitefinityMessage)
        {
            if (sitefinityMessage == null)
            {
                throw new ArgumentNullException(nameof(sitefinityMessage));
            }

            sitefinityMessage.UserProperties.TryGetValue(MessageProperty.Action, out var messagePropertyActionType);
            sitefinityMessage.UserProperties.TryGetValue(MessageProperty.ContentType, out var messagePropertyContentType);

            if (!Enum.TryParse<MessageActionType>(messagePropertyActionType?.ToString(), out var messageActionType))
            {
                throw new ArgumentOutOfRangeException(nameof(messagePropertyActionType), $"Invalid message action '{messagePropertyActionType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageActionType)))}'");
            }

            if (!Enum.TryParse<MessageContentType>(messagePropertyContentType?.ToString(), out var messageContentType))
            {
                throw new ArgumentOutOfRangeException(nameof(messagePropertyContentType), $"Invalid message content type '{messagePropertyContentType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageContentType)))}'");
            }

            var messageBody = Encoding.UTF8.GetString(sitefinityMessage?.Body);
            if (string.IsNullOrWhiteSpace(messageBody))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(sitefinityMessage));
            }

            switch (messageActionType)
            {
                case MessageActionType.Deleted:
                    await ProcessDeleted(messageContentType, messageBody).ConfigureAwait(false);
                    break;
                case MessageActionType.Published:
                    var sequenceNumber = sitefinityMessage.SystemProperties.SequenceNumber;
                    await ProcessPublished(messageContentType, messageBody, sequenceNumber).ConfigureAwait(false);
                    break;
            }
        }

        private async Task ProcessDeleted(MessageContentType messageContentType, string messageBody)
        {
            switch (messageContentType)
            {
                case MessageContentType.JobProfile:
                    var jobProfileServiceBusDeleteSaveModel = ConvertOrThrow<JobProfileServiceBusDeleteSaveModel>(messageBody);
                    await messageProcessor.Delete(jobProfileServiceBusDeleteSaveModel.JobProfileId).ConfigureAwait(false);
                    break;
            }
        }

        private async Task ProcessPublished(MessageContentType messageContentType, string messageBody, long sequenceNumber)
        {
            switch (messageContentType)
            {
                case MessageContentType.JobProfile:
                    var jobProfileServiceBusSaveModel = ConvertOrThrow<JobProfileServiceBusSaveModel>(messageBody);
                    jobProfileServiceBusSaveModel.SequenceNumber = sequenceNumber;
                    await messageProcessor.Save(jobProfileServiceBusSaveModel).ConfigureAwait(false);
                    break;
            }
        }

        private T ConvertOrThrow<T>(string messageBody)
        {
            var result = JsonConvert.DeserializeObject<T>(messageBody);
            if (result == null)
            {
                throw new InvalidOperationException($"Message body cannot be converted to {nameof(T)}");
            }

            return result;
        }
    }
}
