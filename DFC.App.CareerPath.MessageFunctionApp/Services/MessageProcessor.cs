using AutoMapper;
using DFC.App.CareerPath.Common.Constants;
using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Common.Services;
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
        private readonly ILogService logService;
        private readonly ICorrelationIdProvider correlationIdProvider;

        public MessageProcessor(IHttpClientFactory httpClientFactory, IMapper mapper, ILogService logService, ICorrelationIdProvider correlationIdProvider)
        {
            this.httpClient = httpClientFactory.CreateClient(nameof(MessageProcessor));
            this.mapper = mapper;
            this.logService = logService;
            this.correlationIdProvider = correlationIdProvider;
        }

        public async Task<HttpStatusCode> Delete(Guid jobProfileId)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment/", jobProfileId);
            LogInformation($"{nameof(Delete)}. Starting a DELETE operation on {uri}");
            ConfigureHttpClient(httpClient);
            var response = await httpClient.DeleteAsync(uri).ConfigureAwait(false);
            LogInformation($"{nameof(Delete)}. Finished DELETE operation on {uri} with response {response}");
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
            LogInformation($"{nameof(Update)}. Starting a POST operation on {uri}");
            ConfigureHttpClient(httpClient);
            var response = await httpClient.PostAsJsonAsync(uri, careerPathSegmentModel).ConfigureAwait(false);
            LogInformation($"{nameof(Update)}. Finished a POST operation on {uri} with response {response}");
            return response;
        }

        private async Task<HttpResponseMessage> Update(CareerPathSegmentModel careerPathSegmentModel)
        {
            var uri = string.Concat(httpClient.BaseAddress, "segment");
            LogInformation($"{nameof(Update)}. Starting a PUT operation on {uri}");
            ConfigureHttpClient(httpClient);
            var response = await httpClient.PutAsJsonAsync(uri, careerPathSegmentModel).ConfigureAwait(false);
            LogInformation($"{nameof(Update)}. Finished a PUT operation on {uri} with response {response}");
            return response;
        }

        private void LogInformation(string message)
        {
            logService.LogInformation(message);
        }

        private void ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
        }
    }
}
