using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading;

namespace DFC.App.RelatedCareers.Tests.Common.APISupport
{
    public enum ContentType
    {
        Json,
        Html,
    }

    public class GetRequest
    {
        public GetRequest(string endpoint)
        {
            this.Request = new RestRequest(endpoint, Method.GET);
        }

        private RestRequest Request { get; set; }

        public void AddVersionHeader(string version)
        {
            Request.AddHeader("version", version);
        }

        public void AddApimKeyHeader(string apimSubscriptionKey)
        {
            Request.AddHeader("Ocp-Apim-Subscription-Key", apimSubscriptionKey);
        }

        public void AddAcceptHeader(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json:
                    Request.AddHeader("Accept", "application/json");
                    break;

                case ContentType.Html:
                    Request.AddHeader("Accept", "text/html");
                    break;
            }
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
