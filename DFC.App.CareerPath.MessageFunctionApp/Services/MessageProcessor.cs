using AutoMapper;
using DFC.App.CareerPath.Data.Models;
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
            var careerPathSegmentModel = mapper.Map<CareerPathSegmentModel>(message);
            var response = await Update(careerPathSegmentModel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response = await Create(careerPathSegmentModel).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }

        private async Task<HttpResponseMessage> Create(CareerPathSegmentModel careerPathSegmentModel)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment");
            return await httpClient.PostAsJsonAsync(uri, careerPathSegmentModel).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> Update(CareerPathSegmentModel careerPathSegmentModel)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment");
            return await httpClient.PutAsJsonAsync(uri, careerPathSegmentModel).ConfigureAwait(false);
        }
    }
}
