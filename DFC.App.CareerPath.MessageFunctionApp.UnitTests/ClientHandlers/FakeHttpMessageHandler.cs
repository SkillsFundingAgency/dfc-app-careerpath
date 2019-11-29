using DFC.App.CareerPath.MessageFunctionApp.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.ClientHandlers
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly IFakeHttpRequestSender fakeHttpRequestSender;

        public FakeHttpMessageHandler(IFakeHttpRequestSender fakeHttpRequestSender)
        {
            this.fakeHttpRequestSender = fakeHttpRequestSender;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(fakeHttpRequestSender.Send(request));
        }
    }
}