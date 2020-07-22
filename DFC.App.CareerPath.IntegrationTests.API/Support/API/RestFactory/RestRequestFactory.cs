using DFC.App.CareerPath.Tests.IntegrationTests.API.Support.API.RestFactory.Interface;
using RestSharp;

namespace DFC.App.CareerPath.Tests.IntegrationTests.API.Support.API.RestFactory
{
    internal class RestRequestFactory : IRestRequestFactory
    {
        public IRestRequest Create(string urlSuffix)
        {
            return new RestRequest(urlSuffix);
        }
    }
}