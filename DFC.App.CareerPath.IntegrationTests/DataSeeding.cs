using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.CareerPath.IntegrationTests
{
    public static class DataSeeding
    {
        public static void SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory, Guid articleGuid, string article, DateTime created)
        {
            const string url = "/segment";
            var models = new List<CareerPathSegmentModel>()
            {
                new CareerPathSegmentModel()
                {
                    DocumentId = articleGuid,
                    CanonicalName = article,
                    Created = created,
                    Data = new CareerPathSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C16B389D-91AD-4F3D-2485-9F7EE953AFE4"),
                    CanonicalName = $"{article}-2",
                    Data = new CareerPathSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-3F66-1B2DB192177E"),
                    CanonicalName = $"{article}-3",
                    Data = new CareerPathSegmentDataModel
                    {
                        Updated = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
            };

            var client = factory?.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            models.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()));
        }
    }
}
