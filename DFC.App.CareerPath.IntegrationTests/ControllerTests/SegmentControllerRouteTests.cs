using DFC.App.CareerPath.Data.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.IntegrationTests.ControllerTests
{
    [Trait("Integration Tests", "Segment Controller Tests")]
    public class SegmentControllerRouteTests : BaseControllerRouteTests, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public SegmentControllerRouteTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, DefaultArticleGuid, DefaultArticleName, DefaultArticleCreated);
        }

        public static IEnumerable<object[]> SegmentContentRouteData => new List<object[]>
        {
            new object[] { "/Segment" },
            new object[] { $"/Segment/{DefaultArticleName}" },
            new object[] { $"/Segment/{DefaultArticleName}/contents" },
        };

        public static IEnumerable<object[]> MissingSegmentContentRouteData => new List<object[]>
        {
            new object[] { $"/Segment/invalid-segment-name" },
        };

        [Theory]
        [MemberData(nameof(SegmentContentRouteData))]
        public async Task GetSegmentHtmlContentEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Html));

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal($"{MediaTypeNames.Text.Html}; charset={Encoding.UTF8.WebName}", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(MissingSegmentContentRouteData))]
        public async Task GetSegmentHtmlContentEndpointsReturnNoContent(string url)
        {
            // Arrange
            var uri = new Uri(url, UriKind.Relative);
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.GetAsync(uri).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task PostSegmentEndpointsReturnCreated()
        {
            // Arrange
            const string url = "/segment";
            var documentId = Guid.NewGuid();
            var careerPathSegmentModel = new CareerPathSegmentModel()
            {
                DocumentId = documentId,
                CanonicalName = documentId.ToString().ToLowerInvariant(),
                Markup = "<div>some markup</div>",
                Data = new CareerPathSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                },
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(url, careerPathSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task PostSegmentEndpointsForDefaultArticleRefreshAllReturnOk()
        {
            // Arrange
            const string url = "/segment";
            var careerPathSegmentModel = new CareerPathSegmentModel()
            {
                DocumentId = DefaultArticleGuid,
                CanonicalName = DefaultArticleName,
                Created = DefaultArticleCreated,
                Markup = "<div>some markup</div>",
                Data = new CareerPathSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                },
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.PostAsync(url, careerPathSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutSegmentEndpointsReturnOk()
        {
            // Arrange
            const string url = "/segment";
            var documentId = Guid.NewGuid();
            var careerPathSegmentModel = new CareerPathSegmentModel()
            {
                DocumentId = documentId,
                CanonicalName = documentId.ToString().ToLowerInvariant(),
                Markup = "<div>some markup</div>",
                Data = new CareerPathSegmentDataModel
                {
                    Updated = DateTime.UtcNow,
                },
            };
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            _ = await client.PostAsync(url, careerPathSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Act
            var response = await client.PutAsync(url, careerPathSegmentModel, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteSegmentEndpointsReturnNotFound()
        {
            // Arrange
            var uri = new Uri($"/segment/{Guid.NewGuid()}", UriKind.Relative);
            var client = factory.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            // Act
            var response = await client.DeleteAsync(uri).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}