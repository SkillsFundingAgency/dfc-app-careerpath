﻿using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace DFC.App.RelatedCareers.Tests.Common.APISupport
{
    public class GetRequest
    {
        private RestRequest Request { get; set; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public GetRequest(string endpoint)
        {
            Request = new RestRequest(endpoint, Method.GET);
        }

        public void AddQueryParameter(string name, string value)
        {
            Request.AddParameter(name, value);
        }

        public void AddVersionHeader(string version)
        {
            Request.AddHeader("version", version);
            Headers.Add("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
            Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public void AddAcceptHeader(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json:
                    Request.AddHeader("Accept", "application/json");
                    Headers.Add("Accept", "application/json");
                    break;

                case ContentType.Html:
                    Request.AddHeader("Accept", "text/html");
                    Headers.Add("Accept", "text/html");
                    break;
            }
        }

        public void AddContentType(ContentType contentType)
        {
            switch(contentType)
            {
                case ContentType.Json:
                    Request.AddHeader("Content-Type", "application/json");
                    Headers.Add("Content-Type", "application/json");
                    break;

                case ContentType.Html:
                    Request.AddHeader("Content-Type", "text/html");
                    Headers.Add("Content-Type", "text/html");
                    break;
            }
        }

        public enum ContentType
        {
            Json,
            Html
        }

        public Response<T> Execute<T>()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Response<T> response = new Response<T>();
            IRestResponse rawResponse = null;
            
            new RestClient().ExecuteAsync(Request, (IRestResponse apiResponse) => { 
                rawResponse = apiResponse;
                autoResetEvent.Set();
            });

            autoResetEvent.WaitOne();
            response.HttpStatusCode = rawResponse.StatusCode;
            response.IsSuccessful = rawResponse.IsSuccessful;
            response.ErrorMessage = rawResponse.ErrorMessage;
            response.ResponseStatus = rawResponse.ResponseStatus;
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                response.Data = JsonConvert.DeserializeObject<T>(rawResponse.Content);
            }
            return response;
        }
    }
}
