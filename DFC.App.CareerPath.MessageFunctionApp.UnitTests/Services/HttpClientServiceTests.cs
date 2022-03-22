﻿using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using DFC.App.CareerPath.MessageFunctionApp.UnitTests.ClientHandlers;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.Services
{
    public class HttpClientServiceTests
    {
        private readonly SegmentClientOptions options;
        private readonly ILogService logger;
        private readonly ICorrelationIdProvider correlationIdProvider;
        private Mock<IHttpClientFactory> mockFactory;

        public HttpClientServiceTests()
        {
            options = new SegmentClientOptions
            {
                BaseAddress = new Uri("http://baseAddress"),
                Timeout = TimeSpan.MinValue,
            };

            logger = A.Fake<ILogService>();
            correlationIdProvider = A.Fake<ICorrelationIdProvider>();
        }

        public Mock<IHttpClientFactory> CreateClientFactory(HttpClient httpClient)
        {
            mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            return mockFactory;
        }

        [Fact]
        public async Task PostAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            var result = await httpClientService.PostAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PostAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.PostAsync(GetSegmentModel()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsStatusWhenHttpResponseIsNotFound()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            var result = await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task PutAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            var result = await httpClientService.PutAsync(GetSegmentModel()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsOKStatusCodeWhenHttpResponseIsSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            var result = await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsExceptionWhenHttpResponseIsNotSuccessful()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Unsuccessful content") };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://SomeDummyUrl") };
            var httpClientService = new HttpClientService(options, CreateClientFactory(httpClient).Object, logger, correlationIdProvider);

            // Act
            await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false)).ConfigureAwait(false);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        private CareerPathSegmentModel GetSegmentModel()
        {
            return new CareerPathSegmentModel
            {
                DocumentId = Guid.NewGuid(),
                SequenceNumber = 1,
                SocLevelTwo = "12",
                CanonicalName = "job-1",
                Data = new CareerPathSegmentDataModel
                {
                    LastReviewed = DateTime.UtcNow,
                    Markup = "<h1>some markup</h1>",
                },
            };
        }
    }
}