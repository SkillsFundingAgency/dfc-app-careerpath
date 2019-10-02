using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.PatchModels;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public static class HttpClientService
    {
        public static async Task<CareerPathSegmentDataModel> GetByIdAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, Guid id)
        {
            var endpoint = segmentClientOptions.GetEndpoint.Replace("{0}", id.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<CareerPathSegmentDataModel>(responseString);

                return result;
            }

            return default(CareerPathSegmentDataModel);
        }

        public static async Task<HttpStatusCode> PatchSegmentMarkupAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, CareerPathPatchMarkupSegmentModel careerPathPatchMarkupSegmentModel)
        {
            var endpoint = segmentClientOptions.PatchEndpoint.Replace("{0}", careerPathPatchMarkupSegmentModel.DocumentId.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";
            var request = new HttpRequestMessage(HttpMethod.Patch, url);

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            request.Content = new ObjectContent(typeof(CareerPathPatchMarkupSegmentModel), careerPathPatchMarkupSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            return response.StatusCode;
        }

        public static async Task<HttpStatusCode> DeleteAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, Guid id)
        {
            var endpoint = segmentClientOptions.DeleteEndpoint.Replace("{0}", id.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            return response.StatusCode;
        }
    }
}
