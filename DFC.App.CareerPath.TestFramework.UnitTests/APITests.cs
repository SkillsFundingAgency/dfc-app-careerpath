using DFC.App.CareerPath.FunctionalTests.Model.API;
using DFC.App.CareerPath.FunctionalTests.Model.Support;
using DFC.App.CareerPath.FunctionalTests.Support.API;
using DFC.App.CareerPath.FunctionalTests.Support.API.RestFactory.Interface;
using FakeItEasy;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.API.UnitTests
{
    public class APITests
    {
        private AppSettings appSettings;
        private IRestClientFactory restClientFactory;
        private IRestRequestFactory restRequestFactory;
        private ICareerPathAPI careerPathApi;
        private IRestClient restClient;
        private IRestRequest restRequest;

        public APITests()
        {
            this.appSettings = new AppSettings();
            this.restClientFactory = A.Fake<IRestClientFactory>();
            this.restRequestFactory = A.Fake<IRestRequestFactory>();
            this.restClient = A.Fake<IRestClient>();
            this.restRequest = A.Fake<IRestRequest>();
            A.CallTo(() => this.restClientFactory.Create(A<Uri>.Ignored)).Returns(this.restClient);
            A.CallTo(() => this.restRequestFactory.Create(A<string>.Ignored)).Returns(this.restRequest);
            this.careerPathApi = new CareerPathAPI(this.restClientFactory, this.restRequestFactory, this.appSettings);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task EmptyOrNullIdResultsInNullBeingReturned(string id)
        {
            Assert.Null(await this.careerPathApi.GetById(id).ConfigureAwait(true));
        }

        [Fact]
        public async Task SuccessfulGetRequest()
        {
            var apiResponse = new RestResponse<CareerPathAPIResponse>();
            apiResponse.StatusCode = HttpStatusCode.OK;
            A.CallTo(() => this.restClient.Execute<CareerPathAPIResponse>(A<IRestRequest>.Ignored)).Returns(apiResponse);
            var response = await this.careerPathApi.GetById("id").ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("Accept", "application/json")]
        public async Task AllRequestHeadersAreSet(string headerKey, string headerValue)
        {
            var response = await this.careerPathApi.GetById("id").ConfigureAwait(false);
            A.CallTo(() => this.restRequest.AddHeader(headerKey, headerValue)).MustHaveHappenedOnceExactly();
        }
    }
}
