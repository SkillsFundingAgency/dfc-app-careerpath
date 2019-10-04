﻿using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class JobProfileSegmentRefreshService<TModel> : IJobProfileSegmentRefreshService<TModel>
    {
        private readonly ServiceBusOptions serviceBusOptions;

        public JobProfileSegmentRefreshService(ServiceBusOptions serviceBusOptions)
        {
            this.serviceBusOptions = serviceBusOptions;
        }

        public async Task SendMessageAsync(TModel model)
        {
            var messageJson = JsonConvert.SerializeObject(model);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));
            var topicClient = new TopicClient(serviceBusOptions.ServiceBusConnectionString, serviceBusOptions.TopicName);

            await topicClient.SendAsync(message).ConfigureAwait(false);
        }
    }
}