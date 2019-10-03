﻿using DFC.App.CareerPath.Data.Models;
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
        public static async Task<CareerPathSegmentModel> GetByIdAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, Guid id)
        {
            var endpoint = segmentClientOptions.GetEndpoint.Replace("{0}", id.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<CareerPathSegmentModel>(responseString);

                    return result;
                }
            }

            return default(CareerPathSegmentModel);
        }

        public static async Task<HttpStatusCode> PatchAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, CareerPathPatchSegmentModel careerPathPatchSegmentModel, Guid documentId)
        {
            var endpoint = segmentClientOptions.PatchEndpoint.Replace("{0}", documentId.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";

            using (var request = new HttpRequestMessage(HttpMethod.Patch, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(CareerPathPatchSegmentModel), careerPathPatchSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return response.StatusCode;
            }
        }

        public static async Task<HttpStatusCode> PostAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, CareerPathSegmentModel careerPathSegmentModel)
        {
            var endpoint = segmentClientOptions.PostEndpoint;
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                request.Content = new ObjectContent(typeof(CareerPathSegmentModel), careerPathSegmentModel, new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return response.StatusCode;
            }
        }

        public static async Task<HttpStatusCode> DeleteAsync(HttpClient httpClient, SegmentClientOptions segmentClientOptions, Guid id)
        {
            var endpoint = segmentClientOptions.DeleteEndpoint.Replace("{0}", id.ToString().ToLowerInvariant(), System.StringComparison.OrdinalIgnoreCase);
            var url = $"{segmentClientOptions.BaseAddress}{endpoint}";

            using (var request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);

                return response.StatusCode;
            }
        }
    }
}
