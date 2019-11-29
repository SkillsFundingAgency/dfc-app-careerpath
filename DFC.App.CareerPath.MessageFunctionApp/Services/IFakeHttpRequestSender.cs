using System.Net.Http;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}