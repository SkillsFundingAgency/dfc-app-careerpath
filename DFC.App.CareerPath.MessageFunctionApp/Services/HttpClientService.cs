using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.Logger.AppInsights.Constants;
using DFC.Logger.AppInsights.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly SegmentClientOptions segmentClientOptions;
        private readonly HttpClient httpClient;
        private readonly ILogService logger;
        private readonly ICorrelationIdProvider correlationIdProvider;

        public HttpClientService(SegmentClientOptions segmentClientOptions, HttpClient httpClient, ILogService logger, ICorrelationIdProvider correlationIdProvider)
        {
            this.segmentClientOptions = segmentClientOptions;
            this.httpClient = httpClient;
            this.logger = logger;
            this.correlationIdProvider = correlationIdProvider;
        }

        public async Task<HttpStatusCode> PostAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(CareerPathSegmentModel), careerPathSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                ConfigureHttpClient();
                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for POST, Id: {careerPathSegmentModel?.DocumentId}.");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> PutAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment");

            using (var content = new ObjectContent(typeof(CareerPathSegmentModel), careerPathSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json))
            {
                ConfigureHttpClient();
                var response = await httpClient.PutAsync(url, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for Put type {typeof(CareerPathSegmentModel)}, Id: {careerPathSegmentModel?.DocumentId}");
                    response.EnsureSuccessStatusCode();
                }

                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var url = new Uri($"{segmentClientOptions?.BaseAddress}segment/{id}");
            ConfigureHttpClient();
            var response = await httpClient.DeleteAsync(url).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logger.LogError($"Failure status code '{response.StatusCode}' received with content '{responseContent}', for DELETE, Id: {id}.");
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode;
        }

        private void ConfigureHttpClient()
        {
            if (!httpClient.DefaultRequestHeaders.Contains(HeaderName.CorrelationId))
            {
                httpClient.DefaultRequestHeaders.Add(HeaderName.CorrelationId, correlationIdProvider.CorrelationId);
            }
        }
    }
}