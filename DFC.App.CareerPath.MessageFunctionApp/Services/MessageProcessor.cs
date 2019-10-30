using AutoMapper;
using DFC.App.CareerPath.Data.Models.ServiceBusModels.Save;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public MessageProcessor(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient(nameof(MessageProcessor));
            this.mapper = mapper;
        }

        public async Task<HttpStatusCode> Delete(Guid jobProfileId)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment/", jobProfileId);
            var response = await httpClient.DeleteAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> Save(JobProfileServiceBusSaveModel message)
        {
            var response = await Update(message).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response = await Create(message).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }

        private async Task<HttpResponseMessage> Create(JobProfileServiceBusSaveModel model)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment");
            return await httpClient.PostAsJsonAsync(uri, model).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> Update(JobProfileServiceBusSaveModel model)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment");
            return await httpClient.PutAsJsonAsync(uri, model).ConfigureAwait(false);
        }
    }
}
