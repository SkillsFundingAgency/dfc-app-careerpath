using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.IntegrationTests.ControllerTests
{
    [Trait("Category", "Integration")]
    public class SegmentControllerRouteTests : IClassFixture<CustomWebApplicationFactory<DFC.App.CareerPath.Startup>>
    {
        private const string DefaultArticleName = "segment-article";

        private readonly CustomWebApplicationFactory<DFC.App.CareerPath.Startup> factory;

        public SegmentControllerRouteTests(CustomWebApplicationFactory<DFC.App.CareerPath.Startup> factory)
        {
            this.factory = factory;

            DataSeeding.SeedDefaultArticle(factory, DefaultArticleName);
        }

        public static IEnumerable<object[]> SegmentContentRouteData => new List<object[]>
        {
            new object[] { "/Segment" },
            new object[] { $"/Segment/{DefaultArticleName}" },
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
    }
}