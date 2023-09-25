using DFC.App.CareerPath.Data.Contracts;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class JobProfileSegmentRefreshService<TModel> : IJobProfileSegmentRefreshService<TModel>
    {
        private const int BatchSize = 500;
        private readonly ITopicClient topicClient;
        private readonly ICorrelationIdProvider correlationIdProvider;
        private readonly ILogService logService;

        public JobProfileSegmentRefreshService(ITopicClient topicClient, ICorrelationIdProvider correlationIdProvider, ILogService logService)
        {
            this.topicClient = topicClient;
            this.correlationIdProvider = correlationIdProvider;
            this.logService = logService;
        }

        public async Task SendMessageAsync(TModel model)
        {
            logService.LogInformation($"{nameof(SendMessageAsync)} has been called");

            var message = CreateMessage(model);
            await topicClient.SendAsync(message).ConfigureAwait(false);
        }

        public async Task SendMessageListAsync(IList<TModel> models)
        {
            logService.LogInformation($"{nameof(SendMessageListAsync)} has been called");

            // List is batched to avoid exceeding the Service Bus size limit on DEV and SIT of 256KB
            if (models != null)
            {
                var listOfMessages = new List<Message>();
                listOfMessages.AddRange(models.Select(CreateMessage));
                for (var i = 0; i < listOfMessages.Count; i += BatchSize)
                {
                    var batchedList = listOfMessages.Skip(i).Take(BatchSize).ToList();
                    await topicClient.SendAsync(batchedList).ConfigureAwait(false);
                }
            }
            else
            {
                logService.LogInformation($"{nameof(SendMessageListAsync)} has been called with a null list of models {nameof(models)}");
            }
        }

        private Message CreateMessage(TModel model)
        {
            logService.LogInformation($"{nameof(CreateMessage)} has been called");

            var messageJson = JsonConvert.SerializeObject(model);
            return new Message(Encoding.UTF8.GetBytes(messageJson))
            {
                CorrelationId = correlationIdProvider.CorrelationId,
            };
        }
    }
}